﻿using System;
using System.Security.Cryptography;
using System.Text;
using System.IO;

/// <summary>
///myEncryption 数据加密类
/// </summary>
public class myEncryption
{
    private static SymmetricAlgorithm mobjCryptoService = new RijndaelManaged();
    private static string Key = "Guz(%&hj7x89H$yuBI0456FtmaT5&fvHUFCy76*h%(HilJ$lhj!y6&(*jkP87jH7";
    public myEncryption()
    {
        //
        //TODO: 在此处添加构造函数逻辑
        //
    }
    ///   <summary>   
    ///   获得密钥   
    ///   </summary>   
    ///   <returns>密钥</returns>
    private static byte[] GetLegalKey()
    {
        string sTemp = Key;
        mobjCryptoService.GenerateKey();
        byte[] bytTemp = mobjCryptoService.Key;
        int KeyLength = bytTemp.Length;
        if (KeyLength < sTemp.Length)
        {
            sTemp = sTemp.Substring(0, KeyLength);
        }
        else
        {
            sTemp = sTemp.PadRight(KeyLength, ' ');
        }
        return ASCIIEncoding.ASCII.GetBytes(sTemp);
    }

    ///   <summary>   
    ///   获得初始向量IV   
    ///   </summary>   
    ///   <returns>初试向量IV</returns>   
    private static byte[] GetLegalIV()
    {
        string sTemp = "E4ghj*Ghg7!rNIfb&95GUY86GfghUb#er57HBh(u%g6HJ($jhWk7&!hg4ui%$hjk";
        mobjCryptoService.GenerateIV();
        byte[] bytTemp = mobjCryptoService.IV;
        int IVLength = bytTemp.Length;
        if (sTemp.Length > IVLength)
            sTemp = sTemp.Substring(0, IVLength);
        else
            sTemp = sTemp.PadRight(IVLength, ' ');
        return ASCIIEncoding.ASCII.GetBytes(sTemp);
    }

    ///   <summary>   
    ///   加密方法   
    ///   </summary>   
    ///   <param   name="Source">待加密的串</param>   
    ///   <returns>经过加密的串</returns>   
    public string Encrypto(string Source)
    {
        byte[] bytIn = UTF8Encoding.UTF8.GetBytes(Source);
        MemoryStream ms = new MemoryStream();
        mobjCryptoService.IV = GetLegalIV();
        mobjCryptoService.Key = GetLegalKey();
        ICryptoTransform encrypto = mobjCryptoService.CreateEncryptor();
        CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Write);
        cs.Write(bytIn, 0, bytIn.Length);
        cs.FlushFinalBlock();
        ms.Close();
        byte[] bytOut = ms.ToArray();
        return Convert.ToBase64String(bytOut);
    }

    ///   <summary>   
    ///   解密方法   
    ///   </summary>   
    ///   <param   name="Source">待解密的串</param>   
    ///   <returns>经过解密的串</returns>   
    public string Decrypto(string Source)
    {
        byte[] bytIn = Convert.FromBase64String(Source);
        MemoryStream ms = new MemoryStream(bytIn, 0, bytIn.Length);
        mobjCryptoService.Key = GetLegalKey();
        mobjCryptoService.IV = GetLegalIV();
        ICryptoTransform encrypto = mobjCryptoService.CreateDecryptor();
        CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Read);
        StreamReader sr = new StreamReader(cs);
        return sr.ReadToEnd();
    }

}
