using System;
using System.Text;
using UnityEngine;
using Newtonsoft.Json.Linq;
using UnityEngine.SceneManagement;

public static class JwtDecoder
{
    public static int GetPlayerIdFromToken(bool goToLoginIfExpired = true)
    {
        if (JwtIsExpired())
        {
            if (goToLoginIfExpired)
            {
                Debug.Log("JWT is expired! Redirecting to login..");
                PlayerPrefs.DeleteKey("JWT");
                SceneManager.LoadScene("Login");
            }
            else Debug.Log("JWT is expired!");
            return -2; // Force re-login or refresh token
        }

        string decodedPayload = GetDecodedPayload();
        if (decodedPayload == null) return -1;

        JObject json = JObject.Parse(decodedPayload);
        return json["playerId"]?.Value<int>() ?? -1;
    }

    public static string GetEmailFromToken()
    {
        if (JwtIsExpired())
        {
            Debug.LogError("JWT is expired! Redirecting to login.");
            return string.Empty; // Force re-login or refresh token
        }

        string decodedPayload = GetDecodedPayload();
        if (decodedPayload == null) return string.Empty;

        JObject json = JObject.Parse(decodedPayload);
        return json["email"]?.Value<string>() ?? string.Empty;
    }

    // -------------- Helper Methods ---------------
    private static bool JwtIsExpired()
    {
        string decodedPayload = GetDecodedPayload();
        if (string.IsNullOrEmpty(decodedPayload)) return true;

        JObject json = JObject.Parse(decodedPayload);

        long exp = json["exp"]?.Value<long>() ?? 0; // Expiration time (Unix timestamp)
        long currentUnixTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds(); // Current time

        return exp < currentUnixTime; // True if expired
    }

    private static string GetDecodedPayload()
    {
        string payload = GetPayload();
        if (string.IsNullOrEmpty(payload))
        {
            // Debug.Log("JWT Payload is null.");
            return null;
        }

        string decodedPayload = DecodeBase64(payload);
        if (string.IsNullOrEmpty(decodedPayload))
        {
            // Debug.Log("Decoded JWT Payload is empty.");
            return null;
        }

        return decodedPayload;
    }

    private static string GetPayload()
    {
        string[] parts = GetJwtParts();
        if (parts == null) return null;

        return parts[1];    // The middle part is the payload
    }

    private static string[] GetJwtParts()
    {
        string token = PlayerPrefs.GetString("JWT", "");
        if (string.IsNullOrEmpty(token))
        {
            // Debug.Log("JWT token is missing!");
            return null;
        }

        try
        {
            string[] parts = token.Split('.');
            if (parts.Length != 3)
            {
                Debug.LogError("Invalid JWT format.");
                return null;
            }
            return parts;
        }
        catch (Exception ex)
        {
            Debug.LogError("Error decoding JWT: " + ex.Message);
            return null;
        }
    }

    private static string DecodeBase64(string base64)
    {
        if (string.IsNullOrEmpty(base64))
        {
            Debug.LogError("Base64 input is empty.");
            return null;
        }

        base64 = base64.Replace('-', '+').Replace('_', '/'); // Fix Base64 URL encoding
        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }

        try
        {
            byte[] data = Convert.FromBase64String(base64);
            return Encoding.UTF8.GetString(data);
        }
        catch (Exception ex)
        {
            Debug.LogError("Error decoding Base64: " + ex.Message);
            return null;
        }
    }
}