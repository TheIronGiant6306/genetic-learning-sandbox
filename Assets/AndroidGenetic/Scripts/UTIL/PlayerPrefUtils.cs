using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefUtils {


	// check whether key exists
	public static bool hasKey(string key) {
		return PlayerPrefs.HasKey(key);
	}
		

	// boolean 
	public static void setBool(string key, bool booleanValue) {
		PlayerPrefs.SetInt(key, booleanValue ? 1 : 0);
	}

	public static bool getBool(string key)  {
		return PlayerPrefs.GetInt(key) == 1 ? true : false;
	}

	// return boolean value if it exists, otherwise return defaultValue
	public static bool getBool(string name, bool defaultValue) {
		if(PlayerPrefs.HasKey(name)) {
			return getBool(name);
		}

		return defaultValue;
	}


	// integer
	public static void setInt(string key, int intValue) {
		PlayerPrefs.SetInt(key, intValue);
	}

	public static int getInt(string key)  {
		return PlayerPrefs.GetInt(key);
	}

	public static int getInt(string key, int defaultValue) {
		if(PlayerPrefs.HasKey(key)) {
			return getInt(key);
		}

		return defaultValue;
	}


	// string
	public static void setString(string key, string stringValue) {
		PlayerPrefs.SetString(key, stringValue);
	}

	public static string getString(string key)  {
		return PlayerPrefs.GetString(key);
	}

	public static string getString(string key, string defaultValue) {
		if(PlayerPrefs.HasKey(key)) {
			return getString(key);
		}

		return defaultValue;
	}
}
