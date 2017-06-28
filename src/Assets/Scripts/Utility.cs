using System;
using System.Text;
using System.Security.Cryptography;

public class Utility{

	public static string userId = null;

	//Create new identifier string
	public static string NewID(){
		return Guid.NewGuid().ToString();
	}

	//Create MD5 hash from input string
	public static string MD5(string input){
		MD5 md5 = System.Security.Cryptography.MD5.Create();
		byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
		byte[] hash = md5.ComputeHash(inputBytes);
		StringBuilder output = new StringBuilder();
		for (int i = 0; i < hash.Length; i++) {
			output.Append(hash[i].ToString("X2"));
		}
		return output.ToString();
	}
}