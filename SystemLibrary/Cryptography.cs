using System;
using System.Text;
using System.Security.Cryptography;
using System.ComponentModel;
using System.IO;
using System.Collections;
using System.Xml;
using System.Web;

namespace WB.SYSTEM
{
    public class UtilEncrypt
    {
        #region Cosntructor
        /// <summary>
        /// Creates an instance of the necessary objects.
        /// </summary>
        public UtilEncrypt()
        {

        }
        #endregion Cosntructor

        #region Methods
        /// <summary>
        /// Encrypt will create an instance of the ICryptoTransform, and 
        /// Return/Execute the Transform method.
        /// </summary>
        /// <param name="inputvar">Byte array holding the value to be encrypted.</param>
        /// <returns>Byte array storing the holding value.</returns>
        public static string DePass(string connectionString)
        {
            try
            {
                string[] arrStr = connectionString.Split(char.Parse(";"));
                int iCount = arrStr.GetUpperBound(0);
                for (int i = 0; i <= iCount; i++)
                {
                    string EnPass = arrStr[i].Trim();

                    if (EnPass.IndexOf("pwd=") >= 0 || EnPass.IndexOf("PWD=") >= 0)
                    {
                        string DePass = EnPass.Replace("pwd=", "").Replace("PWD=", "");
                        DePass = DES.Decrypt(DePass, true, "");
                        connectionString = connectionString.Replace(EnPass, "pwd=" + DePass);
                        break;
                    }
                    else if (EnPass.IndexOf("Password=") >= 0 || EnPass.IndexOf("PASSWORD=") >= 0)
                    {
                        string DePass = EnPass.Replace("Password=", "").Replace("PASSWORD=", "");
                        DePass = DES.Decrypt(DePass, true, "");
                        connectionString = connectionString.Replace(EnPass, "Password=" + DePass);
                        break;
                    }

                }

                return connectionString;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion Methods

    }
    /// <summary>
    /// Summary description for Cryptography.
    /// </summary>
    public class TripleDES
	{
		#region Variables
		/// <summary>
		/// Global variable to hold the Triple DES Crypto Provider.
		/// </summary>
		private TripleDESCryptoServiceProvider des;
		/// <summary>
		/// Byte array that stores the key.
		/// </summary>
		private byte[] key;
		/// <summary>
		/// Byte array that stores the Initialization Vector.
		/// </summary>
		private byte[] iv;
		#endregion Variables

		#region Properties
		/// <summary>
		/// Property to set the value of the key.
		/// </summary>
		public byte[] Key 
		{
			get { return key; }
			set { key = value; }
		}
		/// <summary>
		/// Property to set the value of the Initialization Vector.
		/// </summary>
		public byte[] IV 
		{
			get { return iv; }
			set { iv = value; }
		}

		/// <summary>
		/// SetKeys will create a 192 bit key and 64 bit IV based on 
		/// two MD5 methods found in another article (http://www.aspalliance.com/535)
		/// </summary>
		public string SetKeys 
		{
			set 
			{
				// create the byte arrays needed to create the key and iv.
				byte[] md5key;
				byte[] hashedkey;

				// for ease of preparation, we'll utilize code found in a different 
				// article.  http://www.aspalliance.com/535 
				md5key = MD5Encryption(value);
				hashedkey = MD5SaltedHashEncryption(value);

				// loop to transfer the keys.
				for (int i=0; i < hashedkey.Length; i++) 
				{
					key[i] = hashedkey[i];
				}

				// create the start and mid portion of the hashed key
				int startcount = hashedkey.Length; /* always 128 */
				int midcount = md5key.Length / 2; /* always 64 */

				// loop to fill in the rest of the key, and create 
				// the IV with the remaining.
				for (int i = midcount; i < md5key.Length; i++) 
				{
					key[startcount + (i - midcount)] = md5key[i];
					iv[i - midcount] = md5key[i - midcount];
				}

				// clean up resources.
				md5key = null;
				hashedkey = null;
			}
		}

		/// <summary>
		/// Gets the current CipherMode for the TripleDes Provider.
		/// <see cref="System.Security.Cryptography.CipherMode"/>
		/// </summary>
		public System.Security.Cryptography.CipherMode GetCipherMode 
		{
			get { return des.Mode; }
		}
		/// <summary>
		/// Sets the CipherMode by translating the enum found in this class
		/// and sets the appropriate CipherMode enum value to the Mode of 
		/// the TripleDes Provider.
		/// <see cref="KraGiE.TripleDES.CipherMode"/>
		/// <see cref="System.Security.Cryptography.CipherMode"/>
		/// </summary>
		public WB.SYSTEM.TripleDES.CipherMode SetCipherMode 
		{
			set { des.Mode = this.TranslateCipherMode(value); }
		}

		#endregion Properties

		#region Cosntructor
		/// <summary>
		/// Creates an instance of the necessary objects.
		/// </summary>
		public TripleDES()
		{
			des = new TripleDESCryptoServiceProvider();
			iv = new byte[8];
			key = new byte[24];
		}
		#endregion Cosntructor

		#region Methods 
		/// <summary>
		/// Encrypt will create an instance of the ICryptoTransform, and 
		/// Return/Execute the Transform method.
		/// </summary>
		/// <param name="inputvar">Byte array holding the value to be encrypted.</param>
		/// <returns>Byte array storing the holding value.</returns>
		public byte[] Encrypt(byte[] inputvar) 
		{
			return Transform(inputvar, des.CreateEncryptor(key, iv));
		}

		/// <summary>
		/// Encrypt will create a byte array, and call it's overload that 
		/// will create the ICryptoTransform to return the needed output.
		/// </summary>
		/// <param name="inputvar">String that needs to be encrypted.</param>
		/// <returns>The encrypted value in string format.</returns>
		public string Encrypt(string inputvar) 
		{
			byte[] inputbytes = Encoding.Default.GetBytes(inputvar);
			return Encoding.Default.GetString(Encrypt(inputbytes));
		}

		public string Encrypt(string inputvar,string strKey) 
		{
			if (strKey=="")
				strKey="123ABC";
			SetKeys=strKey;
			return Encrypt(inputvar);
		}
		/// <summary>
		/// Decrypt will create an instance of the ICryptoTransform, and 
		/// Return/Execute the Transform method.
		/// </summary>
		/// <param name="inputvar">Byte array holding the value to be decrypted.</param>
		/// <returns>Byte array holding the decrypted value.</returns>
		public byte[] Decrypt(byte[] inputvar) 
		{
			return Transform(inputvar, des.CreateDecryptor(key, iv));
		}

		/// <summary>
		/// Decrypt will create a byte array, and call it's overload that 
		/// will create the ICryptoTransform to return the needed output.
		/// </summary>
		/// <param name="inputvar">String that needs to be decrypted.</param>
		/// <returns>The decrypted value in string format.</returns>
		public string Decrypt(string inputvar) 
		{
			
			byte[] inputbytes = Encoding.Default.GetBytes(inputvar);
			return Encoding.Default.GetString(Decrypt(inputbytes));
		}

		public string Decrypt(string inputvar,string strKey) 
		{
			if (strKey=="")
				strKey="123ABC";
			SetKeys=strKey;
			return Decrypt(inputvar);
		}
		/// <summary>
		/// Transform will accept the input byte array, and transform it based on 
		/// the type of ICryptTransform created.
		/// 
		/// Special note on the way the streams are managed and created.
		/// <see cref="System.Security.Cryptography.TripleDESCryptoServiceProvider"/>
		/// </summary>
		/// <param name="inputvar">Byte array holding the value to be transformed.</param>
		/// <param name="transform">This will be the outcome of System.</param>
		/// <returns>Byte array holding the transformed value.</returns>
		private byte[] Transform(byte[] inputvar, ICryptoTransform transform) 
		{
			// Declare the output variable.
			byte[] returnvar;
			// MemoryStream to hold the bytes of the output.
			System.IO.MemoryStream stream = new System.IO.MemoryStream(2048);
			// CryptoStream that will be used to actually write the transformation.
			CryptoStream encryptstream = new CryptoStream(stream, transform, CryptoStreamMode.Write);

			// Write the input array values into the crypto stream, and transform.
			encryptstream.Write(inputvar, 0, (int)inputvar.Length);
			encryptstream.FlushFinalBlock();

			// Get the output to return.
			stream.Position = 0;	
			returnvar = new byte[(int)stream.Length];
			stream.Read(returnvar, 0, (int)returnvar.Length);

			// close streams and destroy objects.
			encryptstream.Close();
			stream.Close();
			encryptstream = null;
			stream = null;

			// return the output.
			return returnvar;
		}
    

		#endregion Methods 

		#region Enum & Method
		/// <summary>
		/// These modes are the only ones available for DES based encryption.  
		/// It's identical to the ones found in System.Security.Cryptography.CipherMode 
		/// but it is completely written out rather than abbreviated.
		/// <see cref="System.Security.Cryptography.CipherMode"/>
		/// </summary>
		public enum CipherMode 
		{
			/// <summary>
			/// System.Security.Cryptography.CipherMode.CBC
			/// </summary>
			CipherBlockChaining = 1,
			/// <summary>
			/// System.Security.Cryptography.CipherMode.ECB
			/// </summary>
			ElectronicCodebook = 2,
			/// <summary>
			/// System.Security.Cryptography.CipherMode.OFB
			/// </summary>
			OutputFeedback = 3,
			/// <summary>
			/// System.Security.Cryptography.CipherMode.CFB
			/// </summary>
			CipherFeedback = 4 
		}

		/// <summary>
		/// Could have been done better with mapping, but decided to go with this route so you 
		/// can get a more text like selection in code.
		/// </summary>
		/// <param name="ciphermode">KraGiE.TripleDES.CipherMode value mapped to the corresponding 
		/// System.Security.Cryptography.CipherMode</param>
		/// <returns>System.Security.Cryptography.CipherMode that matches the selected 
		/// KraGiE.TripleDES.CipherMode</returns>
		private System.Security.Cryptography.CipherMode TranslateCipherMode(WB.SYSTEM.TripleDES.CipherMode ciphermode) 
		{
			return (System.Security.Cryptography.CipherMode)Convert.ToInt32(ciphermode);
		}
		#endregion Enum & Method

		#region MD5 method found at http://www.aspalliance.com/535
		/*************************************************************************/
		/// <summary>
		/// Encrypts the string to a byte array using the MD5 Encryption Algorithm.
		/// <see cref="System.Security.Cryptography.MD5CryptoServiceProvider"/>
		/// </summary>
		/// <param name="ToEncrypt">System.String.  Usually a password.</param>
		/// <returns>System.Byte[]</returns>
		public static byte[] MD5Encryption(string ToEncrypt) 
		{
			// Create instance of the crypto provider.
			MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
			// Create a Byte array to store the encryption to return.
			byte[] hashedbytes;
			// Required UTF8 Encoding used to encode the input value to a usable state.
			UTF8Encoding textencoder = new UTF8Encoding();

			// let the show begin.
			hashedbytes = md5.ComputeHash(textencoder.GetBytes(ToEncrypt));

			// Destroy objects that aren't needed.
			md5.Clear();
			md5 = null;

			// return the hased bytes to the calling method.
			return hashedbytes;
		}

		/// <summary>
		/// Encrypts the string to a byte array using the MD5 Encryption 
		/// Algorithm with an additional Salted Hash.
		/// <see cref="System.Security.Cryptography.MD5CryptoServiceProvider"/>
		/// </summary>
		/// <param name="ToEncrypt">System.String.  Usually a password.</param>
		/// <returns>System.Byte[]</returns>
		public static byte[] MD5SaltedHashEncryption(string ToEncrypt) 
		{
			// Create instance of the crypto provider.
			MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
			// Create a Byte array to store the encryption to return.
			byte[] hashedbytes;
			// Create a Byte array to store the salted hash.
			byte[] saltedhash;

			// Required UTF8 Encoding used to encode the input value to a usable state.
			UTF8Encoding textencoder = new UTF8Encoding();

			// let the show begin.
			hashedbytes = md5.ComputeHash(
				textencoder.GetBytes(ToEncrypt));

			// Let's add the salt.
			ToEncrypt += textencoder.GetString(hashedbytes);
			// Get the new byte array after adding the salt.
			saltedhash = md5.ComputeHash(textencoder.GetBytes(ToEncrypt));

			// Destroy objects that aren't needed.
			md5.Clear();
			md5 = null;

			// return the hased bytes to the calling method.
			return saltedhash;
		}
		#endregion MD5 method found at http://www.aspalliance.com/535

	}

	public class RSA
	{
		public RSA()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		public static void RSAGenerateKeyPair(ref string strPrivateKey,ref string strPublicKey)
		{
			try
			{
				System.Security.Cryptography.RSACryptoServiceProvider RSAProvider;
				RSAProvider = new System.Security.Cryptography.RSACryptoServiceProvider( 1024 );
				strPrivateKey = RSAProvider.ToXmlString( true );
				strPublicKey = RSAProvider.ToXmlString( false );
			}
			catch(WB.SYSTEM.ErrorMessage ex)
			{
				WB.SYSTEM.ErrorHandler.Process(ex);
			}
			catch(Exception ex)
			{
				WB.SYSTEM.ErrorHandler.Process(ex);
			}		
		}
		public static string RSAEncrypt(string strKey,string strData)
		{
			try
			{
				string EncryptedData="";
				System.Security.Cryptography.RSACryptoServiceProvider RSAProvider = new System.Security.Cryptography.RSACryptoServiceProvider();
				System.Security.Cryptography.RSAParameters parameters = new System.Security.Cryptography.RSAParameters();
				RSAProvider.FromXmlString( strKey );
				System.Int32 numberOfBlocks =( strData.Length / 32 ) + 1;
				System.Char[] charArray = strData.ToCharArray();
				System.Byte[][] byteBlockArray = new byte[ numberOfBlocks ][];
				System.Int32 incrementer = 0;
				for( System.Int32 i = 1; i <= numberOfBlocks; i++ )
				{
					if( i == numberOfBlocks )
					{byteBlockArray[ i - 1 ] = System.Text.UTF8Encoding.UTF8.GetBytes( charArray, incrementer, charArray.Length - incrementer );}
					else
					{
						byteBlockArray[ i - 1 ] = System.Text.UTF8Encoding.UTF8.GetBytes( charArray, incrementer, 32 );
						incrementer += 32;
					}
				}
				for( System.Int32 j = 0; j < byteBlockArray.Length; j++ )
				{
					EncryptedData+=( System.Convert.ToBase64String( RSAProvider.Encrypt( byteBlockArray[ j ], true ) ) );
				}
				return EncryptedData;
			}
			catch(WB.SYSTEM.ErrorMessage ex)
			{
				WB.SYSTEM.ErrorHandler.Process(ex);
				return "";
			}
			catch(Exception ex)
			{
				WB.SYSTEM.ErrorHandler.Process(ex);
				return "";
			}		
		}
		public static string RSADecrypt(string strKey,string strData)
		{
			try
			{
				System.Security.Cryptography.RSACryptoServiceProvider RSAProvider = new System.Security.Cryptography.RSACryptoServiceProvider();
				System.Security.Cryptography.RSAParameters parameters = new System.Security.Cryptography.RSAParameters();
				RSAProvider.FromXmlString( strKey );
				System.String encryptedBlock = "";
				System.Collections.Queue encryptedBlocks = new System.Collections.Queue();
				while( strData.Length != 0 )
				{
					if( RSAProvider.KeySize == 1024 )
					{
						encryptedBlock = strData.Substring( 0, strData.IndexOf( "=" ) + 1 );
						encryptedBlocks.Enqueue( encryptedBlock );
						strData = strData.Remove( 0, encryptedBlock.Length );
					}
					else
					{
						encryptedBlock = strData.Substring( 0, strData.IndexOf( "==" ) + 2 );
						encryptedBlocks.Enqueue( encryptedBlock );
						strData = strData.Remove( 0, encryptedBlock.Length );
					}
				}
				encryptedBlocks.TrimToSize();
				System.Int32 numberOfBlocks = encryptedBlocks.Count;
				for( System.Int32 i = 1; i <= numberOfBlocks; i++ )
				{
					encryptedBlock =( System.String )encryptedBlocks.Dequeue();
					strData+=( System.Text.UTF8Encoding.UTF8.GetString( RSAProvider.Decrypt( System.Convert.FromBase64String( encryptedBlock ), true ) ) );
				}
				
				
				string DecryptData=strData;

				
				return DecryptData;
			}
			catch(WB.SYSTEM.ErrorMessage ex)
			{
				WB.SYSTEM.ErrorHandler.Process(ex);
				return "";
			}
			catch(Exception ex)
			{
				WB.SYSTEM.ErrorHandler.Process(ex);
				return "";
			}		
		}
	}

    public class Encrypt
    {
        #region Variables

        Byte[] btKey = null;
        Byte[] btIV = null;
        string sKey = null;
     
        #endregion

        #region Properties


        #endregion

        #region Contructor

        public Encrypt()
        {
        }
        public Encrypt(string strKey)
        {
            sKey = strKey;
            btKey = CreateKey(sKey);
            btIV = CreateIV(sKey);

        }

        #endregion

        #region Method


        private byte[] CreateIV(string strKey)
        {
            try
            {
                char[] chrData = strKey.ToCharArray();
                int intLength = chrData.GetUpperBound(0);
                byte[] bytDataToHash = new byte[intLength + 1];

                for (int i = 0; i <= chrData.GetUpperBound(0); i++)
                {
                    bytDataToHash[i] = Convert.ToByte(chrData[i]);
                }

                SHA512Managed SHA512 = new SHA512Managed();
                byte[] bytResult = SHA512.ComputeHash(bytDataToHash);
                byte[] bytIV = new byte[16];

                for (int i = 32; i <= 47; i++)
                {
                    bytIV[i - 32] = bytResult[i];
                }

                return bytIV;
            }
            catch (Exception ex)
            {
                ErrorHandler.Process(ex);
                return null;
            }
        }

        private byte[] CreateKey(string strKey)
        {
            try
            {
                char[] chrData = strKey.ToCharArray();
                int intLength = chrData.GetUpperBound(0);
                byte[] bytDataToHash = new byte[intLength + 1];

                for (int i = 0; i <= chrData.GetUpperBound(0); i++)
                {
                    bytDataToHash[i] = Convert.ToByte(chrData[i]);
                }

                SHA512Managed SHA512 = new SHA512Managed();
                byte[] bytResult = SHA512.ComputeHash(bytDataToHash);
                byte[] bytKey = new byte[32];

                for (int i = 0; i <= 31; i++)
                {
                    bytKey[i] = bytResult[i];
                }
                return bytKey;
            }
            catch (Exception ex)
            {
                ErrorHandler.Process(ex);
                return null;
            }
        }
    
        public static string GenKeyString()
        {
            int LowerBound = 63;
            int UperBound = 96;        
            int ch;
            string EncKey = string.Empty;
            int sKeyLength = 32;

            try
            {
                Random random = new Random();

                for (int i = 0; i <= sKeyLength - 1; i++)
                {
                    ch = int.Parse((((UperBound - LowerBound) * random.NextDouble() + LowerBound).ToString()).Substring(0,2));

                    EncKey += Convert.ToChar(ch);
                }               
                return EncKey;
            }
            catch (Exception ex)
            {
                ErrorHandler.Process(ex);
                return null;
            }

        }

        public static string EncryptPasword(string cleanString)
        {
            string hashedText = null;
            try
            {
                byte[] clearBytes;
                clearBytes = new UnicodeEncoding().GetBytes(cleanString);
                byte[] hashedBytes = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(clearBytes);
                hashedText = BitConverter.ToString(hashedBytes);
                return hashedText;
            }
            catch (Exception ex)
            {
                return hashedText;
                ErrorHandler.Process(ex);
            }
        }

        public static string GenerateVerifyCode()
        {

            string PASSWORD_CHARS_LCASE = "abcdefgijkmnopqrstwxyz";
            string PASSWORD_CHARS_UCASE = "ABCDEFGHJKLMNPQRSTWXYZ";
            string PASSWORD_CHARS_NUMERIC = "23456789";
            int minLength = 6;
            int maxLength = 8;
            try
            {
                // Create a local array containing supported password characters 
                // grouped by types. 
                char[][] charGroups = new char[][] { PASSWORD_CHARS_LCASE.ToCharArray(), PASSWORD_CHARS_UCASE.ToCharArray(), PASSWORD_CHARS_NUMERIC.ToCharArray() };

                // Use this array to track the number of unused characters in each 
                // character group. 
                int[] charsLeftInGroup = new int[charGroups.Length];

                // Initially, all characters in each group are not used. 
                int I;
                for (I = 0; I <= charsLeftInGroup.Length - 1; I++)
                {
                    charsLeftInGroup[I] = charGroups[I].Length;
                }

                // Use this array to track (iterate through) unused character groups. 
                int[] leftGroupsOrder = new int[charGroups.Length];

                // Initially, all character groups are not used. 
                for (I = 0; I <= leftGroupsOrder.Length - 1; I++)
                {
                    leftGroupsOrder[I] = I;
                }

                // Because we cannot use the default randomizer, which is based on the 
                // current time (it will produce the same "random" number within a 
                // second), we will use a random number generator to seed the 
                // randomizer. 

                // Use a 4-byte array to fill it with random bytes and convert it then 
                // to an integer value. 
                byte[] randomBytes = new byte[4];

                // Generate 4 random bytes. 
                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

                rng.GetBytes(randomBytes);

                // Convert 4 bytes into a 32-bit integer value. 
                int seed = ((randomBytes[0] & 127) << 24 | randomBytes[1] << 16 | randomBytes[2] << 8 | randomBytes[3]);

                // Now, this is real randomization. 
                Random random = new Random(seed);

                // This array will hold password characters. 
                char[] password = null;

                // Allocate appropriate memory for the password. 
                if ((minLength < maxLength))
                {
                    password = new char[random.Next(minLength - 1, maxLength) + 1];
                }
                else
                {
                    password = new char[minLength];
                }

                // Index of the next character to be added to password. 
                int nextCharIdx;

                // Index of the next character group to be processed. 
                int nextGroupIdx;

                // Index which will be used to track not processed character groups. 
                int nextLeftGroupsOrderIdx;

                // Index of the last non-processed character in a group. 
                int lastCharIdx;

                // Index of the last non-processed group. Initially, we will skip 
                // special characters. 
                int lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;

                // Generate password characters one at a time. 
                for (I = 0; I <= password.Length - 1; I++)
                {

                    // If only one character group remained unprocessed, process it; 
                    // otherwise, pick a random character group from the unprocessed 
                    // group list. 
                    if ((lastLeftGroupsOrderIdx == 0))
                    {
                        nextLeftGroupsOrderIdx = 0;
                    }
                    else
                    {
                        nextLeftGroupsOrderIdx = random.Next(0, lastLeftGroupsOrderIdx);
                    }

                    // Get the actual index of the character group, from which we will 
                    // pick the next character. 
                    nextGroupIdx = leftGroupsOrder[nextLeftGroupsOrderIdx];

                    // Get the index of the last unprocessed characters in this group. 
                    lastCharIdx = charsLeftInGroup[nextGroupIdx] - 1;

                    // If only one unprocessed character is left, pick it; otherwise, 
                    // get a random character from the unused character list. 
                    if ((lastCharIdx == 0))
                    {
                        nextCharIdx = 0;
                    }
                    else
                    {
                        nextCharIdx = random.Next(0, lastCharIdx + 1);
                    }

                    // Add this character to the password. 
                    password[I] = charGroups[nextGroupIdx][nextCharIdx];

                    // If we processed the last character in this group, start over. 
                    if ((lastCharIdx == 0))
                    {
                        charsLeftInGroup[nextGroupIdx] = charGroups[nextGroupIdx].Length;
                    }
                    // There are more unprocessed characters left. 
                    else
                    {
                        // Swap processed character with the last unprocessed character 
                        // so that we don't pick it until we process all characters in 
                        // this group. 
                        if ((lastCharIdx != nextCharIdx))
                        {
                            char temp = charGroups[nextGroupIdx][lastCharIdx];
                            charGroups[nextGroupIdx][lastCharIdx] = charGroups[nextGroupIdx][nextCharIdx];
                            charGroups[nextGroupIdx][nextCharIdx] = temp;
                        }

                        // Decrement the number of unprocessed characters in 
                        // this group. 
                        charsLeftInGroup[nextGroupIdx] = charsLeftInGroup[nextGroupIdx] - 1;
                    }

                    // If we processed the last group, start all over. 
                    if ((lastLeftGroupsOrderIdx == 0))
                    {
                        lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;
                    }
                    // There are more unprocessed groups left. 
                    else
                    {
                        // Swap processed group with the last unprocessed group 
                        // so that we don't pick it until we process all groups. 
                        if ((lastLeftGroupsOrderIdx != nextLeftGroupsOrderIdx))
                        {
                            int temp = leftGroupsOrder[lastLeftGroupsOrderIdx];
                            leftGroupsOrder[lastLeftGroupsOrderIdx] = leftGroupsOrder[nextLeftGroupsOrderIdx];
                            leftGroupsOrder[nextLeftGroupsOrderIdx] = temp;
                        }

                        // Decrement the number of unprocessed groups. 
                        lastLeftGroupsOrderIdx = lastLeftGroupsOrderIdx - 1;
                    }
                }

                // Convert password characters into a string and return the result. 
                
                string result = new String(password);
                return result;
            }
            catch (Exception ex)
            {
                ErrorHandler.Process(ex);
                return null;
            }
        }

        #endregion

    
    }

    public class URLEncrypt
    {
        #region Variables

        private IntPtr handle;
        private Component component = new Component();
        private bool disposed = false;

        RSAParameters[] prKey = new RSAParameters[2];

        string strKey = "";
        string strURL = "";

        #endregion

        #region Contructor

        public URLEncrypt()
        {            
        }
        public URLEncrypt(string sURL)
        {
            strURL = sURL;
            strKey = RinEncrypt.GenKeyString();
        }

        public URLEncrypt(string sURL, string sKey)
        {
            strURL = sURL;
            strKey = sKey;
        }

        public URLEncrypt(string sURL, string sKey,ArrayList arrParm)
        {
            strURL = sURL + "?" + GenParam(arrParm);
            strKey = sKey;
        }


        #endregion

        #region IDisposable Members

        public URLEncrypt(IntPtr handle)
        {
            this.handle = handle;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    component.Dispose();
                }
                CloseHandle(handle);
                handle = IntPtr.Zero;
            }
            disposed = true;
        }
        [System.Runtime.InteropServices.DllImport("Kernel32")]
        private extern static Boolean CloseHandle(IntPtr handle);

        ~URLEncrypt()
        {
            Dispose(false);
        }

        #endregion

        #region Properties

        public string Key
        {
            get { return strKey; }
            set
            {
                if (value != "")
                {
                    strKey = value;
                }
            }
        }

        public string URL
        {
            get { return strURL; }
            set
            {
                if (value != "")
                {
                    strURL = value;
                }
            }
        }

        #endregion

        #region Method

        public string GenParam(ArrayList arrParm)
        {
            try
            {
                string st = "";
                for (int i = 0; i < arrParm.Count; i += 2)
                {
                    st += arrParm[i].ToString().ToUpper() + "=" + arrParm[i + 1].ToString() + "&";
                }
                st = st.TrimEnd(char.Parse("&"));
                return st;
            }
            catch (Exception ex)
            {
                ErrorHandler.ThrowError(ex);
                return null;
            }
        }        

        public ArrayList GetParam()
        {
            try
            {                
                string sUrl = null;
                string[] QArr;
                int i;
                int iCount;
                ArrayList arrResult = new ArrayList();

                sUrl = DecryptURL(strURL);

                int intLenPath = sUrl.IndexOf("?");
                QArr = sUrl.Substring(intLenPath + 1, sUrl.Length - (intLenPath + 1)).Split(new char[] { '&' });

                iCount = QArr.GetUpperBound(0);
                for (i = 0; i <= iCount; i ++)
                {
                    string[] pArr;
                    pArr = QArr[i].Split(new char[] { '=' });

                    arrResult.Add(pArr[0].ToUpper());
                    arrResult.Add(pArr[1]);
                }

                return arrResult;
            }
            catch (Exception ex)
            {
                ErrorHandler.ThrowError(ex);
                return null;
            }
        }

        public ArrayList GetParam(bool isCrypt)
        {
            try
            {
                //string sUrl = null;
                string[] QArr;
                int i;
                int iCount;
                ArrayList arrResult = new ArrayList();

                if (isCrypt)
                    strURL = DecryptURL(strURL);

                int intLenPath = strURL.IndexOf("?");
                if (intLenPath >= 0)
                {
                    QArr = strURL.Substring(intLenPath + 1, strURL.Length - (intLenPath + 1)).Split(new char[] { '&' });

                    iCount = QArr.GetUpperBound(0);
                    for (i = 0; i <= iCount; i++)
                    {
                        string[] pArr;
                        pArr = QArr[i].Split(new char[] { '=' });

                        arrResult.Add(pArr[0].ToUpper());
                        arrResult.Add(pArr[1]);
                    }
                }

                return arrResult;
            }
            catch (Exception ex)
            {
                //ErrorHandler.ThrowError(ex);
                //ErrorHandler.Process(ex);
                return null;
            }
        }

        public string RequestQueryString(string ParamName)
        {
            string sUrl = null;
            string[] QArr;
            int i;
            int iCount;
            string StrResult = "";

            try
            {
                sUrl = DecryptURL(strURL);

                int intLenPath = sUrl.IndexOf("?");
                QArr = sUrl.Substring(intLenPath + 1, sUrl.Length - (intLenPath + 1)).Split(new char[] { '&' });

                iCount = QArr.GetUpperBound(0);

                for (i = 0; i <= iCount; i++)
                {
                    string[] pArr;
                    pArr = QArr[i].Split(new char[] { '=' });
                    if (pArr.Length == 2)
                    {
                        if (ParamName.ToUpper() == pArr[0].ToUpper())
                        {
                            StrResult = pArr[1];
                            break;
                        }
                    }
                }

                return StrResult;
            }
            catch (ErrorMessage er)
            {
                ErrorHandler.ProcessErr(er);
                return null;
            }
            catch (Exception ex)
            {
                ErrorMessage objErr = new ErrorMessage();
                objErr.ErrorCode = ErrorHandler.EBANK_ALL_CODE;
                objErr.ErrorDesc = ex.Message;
                objErr.ErrorSource = ex.Source;
                throw objErr;
            }
        }
      
        public string DecryptURL2(string strEnc)
        {
            string strEncURL = null;
            string strResult = null;
            string strQuery = null;

            if (strEnc != "")
            {
                strEncURL = strURL;
            }
            else
            {
                strEncURL = strEnc;
            }

            //return strEncURL;

            try
            {
                int intLenPath = strEncURL.IndexOf("?");

                if (intLenPath != -1)
                {
                    strQuery = strEncURL.Substring(intLenPath + 1, strEncURL.Length - (intLenPath + 1));

                    if (strQuery.Length != 0)
                    {
                        RinEncrypt objEnc = new RinEncrypt(strKey);
                        strResult = objEnc.DecryptURL(strQuery);
                    }

                    if (strResult != null)
                    {
                        strResult = strEncURL.Substring(0, intLenPath + 1) + strResult;
                    }
                    else
                    {
                        strResult = strEncURL;
                    }

                    return strResult.Replace("%20", " ");
                }
                else
                {
                    return strEncURL;
                }


            }
            catch (ErrorMessage er)
            {
                ErrorHandler.ProcessErr(er);
                return null;
            }
            catch (Exception ex)
            {
                ErrorMessage objErr = new ErrorMessage();
                objErr.ErrorCode = ErrorHandler.EBANK_ALL_CODE;
                objErr.ErrorDesc = ex.Message;
                objErr.ErrorSource = ex.Source;
                throw objErr;
            }
        }

        public string DecryptURL(string strEnc)
        {
            string strEncURL = null;
            string strResult = null;
            string strKey = null;

            if (strEnc != "")
            {
                strEncURL = strURL;
            }
            else
            {
                strEncURL = strEnc;
            }

            try
            {
                int intLenPath = strEncURL.IndexOf("?");

                if (intLenPath != -1)
                {
                    strKey = strEncURL.Substring(intLenPath + 1, strEncURL.Length - (intLenPath + 1));

                    if (strKey.Length != 0)
                    {
                        //FIX
                        //ArrayList arrParm = (ArrayList)HttpContext.Current.Session["URL_PARM"];
                        ArrayList arrParm = new ArrayList();
                        string strQuery = SysUtils.CString(SysUtils.getURLKey(arrParm, strKey));
                        strResult = strEncURL.Substring(0, intLenPath + 1) + strQuery;
                    }
                    else

                        strResult = strEncURL;
                }
                else
                {
                    strResult = strEncURL;
                }

                return strResult;

            }
            catch (ErrorMessage er)
            {
                ErrorHandler.ProcessErr(er);
                return null;
            }
            catch (Exception ex)
            {
                ErrorMessage objErr = new ErrorMessage();
                objErr.ErrorCode = ErrorHandler.EBANK_ALL_CODE;
                objErr.ErrorDesc = ex.Message;
                objErr.ErrorSource = ex.Source;
                throw objErr;
            }
        }

        public string EncryptURL2(string sURL)
        {
            string strEncURL = null;
            string strResult = null;
            string strQuery = null;

            if (sURL == "")
            {
                strEncURL = strURL;
            }
            else
            {
                strEncURL = sURL;
            }

            try
            {
                int intLenPath = strEncURL.IndexOf("?");

                if (intLenPath != -1)
                {
                    strQuery = strEncURL.Substring(intLenPath + 1, strEncURL.Length - (intLenPath + 1)).Trim();

                    if (strQuery.Length != 0)
                    {
                        RinEncrypt objEnc = new RinEncrypt(strKey);
                        strResult = objEnc.Encrypt(strQuery);
                    }
                    if (strResult != null)
                    {
                        strResult = strEncURL.Substring(0, intLenPath + 1) + strResult;
                    }
                    else
                    {
                        strResult = strEncURL;
                    }
                    return strResult;
                }
                else
                {
                    return strEncURL;
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.Process(ex);
                return null;
            }
        }


        #endregion

    }

    public class DES
    {
        #region Contructor
        public DES()
        {
        }
        #endregion

        #region Method

        public static byte[] EncryptDES(byte[] plainData, string Key)
        {
            try
            {
                byte[] bkey = null;
                bkey = Encoding.Default.GetBytes(Key);

                DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
                DES.Key = bkey;
                DES.IV = bkey;

                ICryptoTransform desEncrypt = DES.CreateEncryptor(DES.Key, DES.IV);
                byte[] encryptedData = desEncrypt.TransformFinalBlock(plainData, 0, plainData.Length);

                return encryptedData;
            }
            catch (Exception ex)
            {
                ErrorHandler.Process(ex);
                return null;
            }
        }

        public static byte[] DecryptDES(byte[] plainData, string Key)
        {
            try
            {
                byte[] bkey = null;
                bkey = Encoding.Default.GetBytes(Key);

                DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
                DES.Key = bkey;
                DES.IV = bkey;

                ICryptoTransform desEncrypt = DES.CreateDecryptor(DES.Key, DES.IV);
                byte[] encryptedData = desEncrypt.TransformFinalBlock(plainData, 0, plainData.Length);

                return encryptedData;
            }
            catch (Exception ex)
            {
                ErrorHandler.Process(ex);
                return null;
            }
        }
        public static byte[] DesEncryptOneBlock(byte[] plainText, byte[] key)
        {
            // Create a new 3DES key.
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            // Set the KeySize = 192 for 168-bit DES encryption.
            // The msb of each byte is a parity bit, so the key length is actually 168 bits.
            //des.KeySize = 192;
            des.Key = key;
            des.Mode = CipherMode.ECB;
            des.Padding = PaddingMode.None;

            ICryptoTransform ic = des.CreateEncryptor();

            byte[] enc = ic.TransformFinalBlock(plainText, 0, 8);

            return enc;
        }

        public static byte[] DesDecryptOneBlock(byte[] plainText, byte[] key)
        {
            // Create a new 3DES key.
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            // Set the KeySize = 192 for 168-bit DES encryption.
            // The msb of each byte is a parity bit, so the key length is actually 168 bits.
            //des.KeySize = 192;
            des.Key = key;
            des.Mode = CipherMode.ECB;
            des.Padding = PaddingMode.None;

            ICryptoTransform ic = des.CreateDecryptor();

            byte[] enc = ic.TransformFinalBlock(plainText, 0, 8);

            return enc;
        }

        public static string Encrypt(string toEncrypt, bool useHashing, string strKey)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            string key = strKey;
            //System.Windows.Forms.MessageBox.Show(key);
            //If hashing use get hashcode regards to your key
            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                //Always release the resources and flush data
                // of the Cryptographic service provide. Best Practice
                hashmd5.Clear();
            }
            else
            {
                keyArray = UTF8Encoding.UTF8.GetBytes(key);
            }

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes.
            //We choose ECB(Electronic code Book)
            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)

            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            //transform the specified region of bytes array to resultArray
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor
            tdes.Clear();
            //Return the encrypted data into unreadable string format
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public static string Decrypt(string cipherString, bool useHashing, string strKey)
        {
            byte[] keyArray;
            //get the byte code of the string

            byte[] toEncryptArray = Convert.FromBase64String(cipherString);

            //Get your key from config file to open the lock!
            string key = strKey;

            if (useHashing)
            {
                //if hashing was used get the hash code with regards to your key
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                //release any resource held by the MD5CryptoServiceProvider

                hashmd5.Clear();
            }
            else
            {
                //if hashing was not implemented get the byte code of the key
                keyArray = UTF8Encoding.UTF8.GetBytes(key);
            }

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes. 
            //We choose ECB(Electronic code Book)

            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor                
            tdes.Clear();
            //return the Clear decrypted TEXT
            return UTF8Encoding.UTF8.GetString(resultArray);
        }
        #endregion
    }

    public class RinEncrypt
    {
        #region Variables

        Byte[] btKey = null;
        Byte[] btIV = null;
        string sKey = null;

        #endregion

        #region Properties


        #endregion

        #region Contructor

        public RinEncrypt()
        {
        }
        public RinEncrypt(string strKey)
        {
            sKey = strKey;
            btKey = CreateKey(sKey);
            btIV = CreateIV(sKey);

        }

        #endregion

        #region Method


        private byte[] CreateIV(string strKey)
        {
            try
            {
                char[] chrData = strKey.ToCharArray();
                int intLength = chrData.GetUpperBound(0);
                byte[] bytDataToHash = new byte[intLength + 1];

                for (int i = 0; i <= chrData.GetUpperBound(0); i++)
                {
                    bytDataToHash[i] = Convert.ToByte(chrData[i]);
                }

                SHA512Managed SHA512 = new SHA512Managed();
                byte[] bytResult = SHA512.ComputeHash(bytDataToHash);
                byte[] bytIV = new byte[16];

                for (int i = 32; i <= 47; i++)
                {
                    bytIV[i - 32] = bytResult[i];
                }

                return bytIV;
            }
            catch (Exception ex)
            {
                ErrorHandler.Process(ex);
                return null;
            }
        }

        private byte[] CreateKey(string strKey)
        {
            try
            {
                char[] chrData = strKey.ToCharArray();
                int intLength = chrData.GetUpperBound(0);
                byte[] bytDataToHash = new byte[intLength + 1];

                for (int i = 0; i <= chrData.GetUpperBound(0); i++)
                {
                    bytDataToHash[i] = Convert.ToByte(chrData[i]);
                }

                SHA512Managed SHA512 = new SHA512Managed();
                byte[] bytResult = SHA512.ComputeHash(bytDataToHash);
                byte[] bytKey = new byte[32];

                for (int i = 0; i <= 31; i++)
                {
                    bytKey[i] = bytResult[i];
                }
                return bytKey;
            }
            catch (Exception ex)
            {
                ErrorHandler.Process(ex);
                return null;
            }
        }

        public string Decrypt(string strDec)
        {

            // Check arguments.
            if (strDec == null || strDec.Length <= 0)
                throw new ArgumentNullException("Encrypt string");
            if (btKey == null || btKey.Length <= 0)
                throw new ArgumentNullException("Key");
            if (btIV == null || btIV.Length <= 0)
                throw new ArgumentNullException("IV");

            // TDeclare the streams used
            // to decrypt to an in memory
            // array of bytes.
            MemoryStream msDecrypt = null;
            CryptoStream csDecrypt = null;
            StreamReader srDecrypt = null;

            // Declare the RijndaelManaged object
            // used to decrypt the data.
            RijndaelManaged aesAlg = null;

            // Declare the string used to hold
            // the decrypted text.
            string strResult = null;

            try
            {
                //
                char[] chrData = strDec.ToCharArray();
                int intLength = chrData.GetUpperBound(0);
                byte[] arrByte = new byte[intLength + 1];

                for (int i = 0; i <= chrData.GetUpperBound(0); i++)
                {
                    arrByte[i] = Convert.ToByte(chrData[i]);
                }
                // Create a RijndaelManaged object
                // with the specified key and IV.
                aesAlg = new RijndaelManaged();

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(btKey, btIV);

                // Create the streams used for decryption.
                msDecrypt = new MemoryStream(arrByte);
                csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
                srDecrypt = new StreamReader(csDecrypt);

                // Read the decrypted bytes from the decrypting stream
                // and place them in a string.
                strResult = srDecrypt.ReadToEnd();
                return strResult;
            }
            catch (Exception ex)
            {
                ErrorHandler.Process(ex);
                return null;
            }
            finally
            {
                // Clean things up.

                // Close the streams.
                if (srDecrypt != null)
                    srDecrypt.Close();
                if (csDecrypt != null)
                    csDecrypt.Close();
                if (msDecrypt != null)
                    msDecrypt.Close();

                // Clear the RijndaelManaged object.
                if (aesAlg != null)
                    aesAlg.Clear();
            }

        }

        public string DecryptURL(string strDec)
        {

            byte[] bytBuffer = new byte[4096];
            CryptoStream fsCrypt;
            int intByte2Read;
            RijndaelManaged cspRijndael = new RijndaelManaged();
            MemoryStream fsIn;
            MemoryStream fsOut = new MemoryStream();
            string strOut;

            byte[] btIn = null;
            try
            {
                btIn = Convert.FromBase64String(strDec);
            }
            catch { return null; }

            byte[] btOut;

            fsIn = new MemoryStream(btIn);
            try
            {
                fsCrypt = new CryptoStream(fsOut, cspRijndael.CreateDecryptor(btKey, btIV), CryptoStreamMode.Write);
                do
                {
                    intByte2Read = fsIn.Read(bytBuffer, 0, 4096);
                    fsCrypt.Write(bytBuffer, 0, intByte2Read);
                }
                while (intByte2Read == 4096);
                fsCrypt.FlushFinalBlock();

                btOut = new byte[fsOut.Length];
                fsOut.Position = 0;
                fsOut.Read(btOut, 0, int.Parse(fsOut.Length.ToString()));
                strOut = Encoding.UTF8.GetString(btOut);

                fsCrypt.Dispose();
                return strOut;
            }
            catch (ErrorMessage er)
            {
                ErrorHandler.ProcessErr(er);
                return null;
            }
            catch (Exception ex)
            {
                ErrorMessage objErr = new ErrorMessage();
                objErr.ErrorCode = ErrorHandler.EBANK_ALL_CODE;
                objErr.ErrorDesc = ex.Message;
                objErr.ErrorSource = ex.Source;
                throw objErr;
            }
            finally
            {
                fsIn.Dispose();
                fsOut.Dispose();
                cspRijndael.Clear();
            }
        }

        public string Encrypt(string strEnc)
        {
            byte[] bytBuffer = new byte[4096];
            CryptoStream fsCrypt;
            int intByte2Read;
            RijndaelManaged cspRijndael = new RijndaelManaged();
            MemoryStream fsIn;
            MemoryStream fsOut = new MemoryStream();
            string strOut;
            byte[] btIn = Encoding.UTF8.GetBytes(strEnc);
            byte[] btOut;

            fsIn = new MemoryStream(btIn);
            try
            {
                fsCrypt = new CryptoStream(fsOut, cspRijndael.CreateEncryptor(btKey, btIV), CryptoStreamMode.Write);

                do
                {
                    intByte2Read = fsIn.Read(bytBuffer, 0, 4096);
                    fsCrypt.Write(bytBuffer, 0, intByte2Read);
                }
                while (intByte2Read == 4096);
                fsCrypt.FlushFinalBlock();

                btOut = new byte[fsOut.Length];
                fsOut.Position = 0;
                fsOut.Read(btOut, 0, int.Parse(fsOut.Length.ToString()));
                strOut = Convert.ToBase64String(btOut);

                fsCrypt.Dispose();

                return strOut;
            }
            catch (Exception ex)
            {
                ErrorHandler.Process(ex);
                return null;
            }
            finally
            {
                fsIn.Dispose();
                fsOut.Dispose();
                cspRijndael.Clear();
            }
        }

        public static string GenKeyString()
        {
            int LowerBound = 63;
            int UperBound = 96;
            int ch;
            string EncKey = string.Empty;
            int sKeyLength = 32;

            try
            {
                Random random = new Random();

                for (int i = 0; i <= sKeyLength - 1; i++)
                {

                    ch = int.Parse((((UperBound - LowerBound) * random.NextDouble() + LowerBound).ToString()).Substring(0, 2));

                    EncKey += Convert.ToChar(ch);
                }

                return EncKey;
            }
            catch (Exception ex)
            {
                ErrorHandler.Process(ex);
                return null;
            }

        }

        #endregion
        
    }

    public class RSAeBank
    {
        #region Variables

        private RSACryptoServiceProvider m_RSAProvider = null;
        private string m_PathPrivateKey;
        private string m_PathPublicKey;

        #endregion

        #region Public Property

        public string PathPrivateKey
        {
            get { return m_PathPrivateKey; }
            set { m_PathPrivateKey = value; }
        }

        public string PathPublicKey
        {
            get { return m_PathPublicKey; }
            set { m_PathPublicKey = value; }
        }

        public RSACryptoServiceProvider RSAProvider
        {
            get { return m_RSAProvider; }
            set { m_RSAProvider = value; }
        }

        #endregion

        #region Constructors

        public RSAeBank()
        {
            //
            // TODO: Add constructor logic here
            //
            RSAProvider = new RSACryptoServiceProvider();

        }
        public RSAeBank(string p_PathPublicKey, string p_PathPrivateKey)
        {
            try
            {
                PathPrivateKey = p_PathPrivateKey;
                PathPublicKey = p_PathPublicKey;
                RSAProvider = new RSACryptoServiceProvider();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Public Method

        public string ReadPrivateKey()
        {
            string m_Modulus = "";
            string m_Exponent = "";
            string m_P = "";
            string m_Q = "";
            string m_DP = "";
            string m_DQ = "";
            string m_InverseQ = "";
            string m_D = "";

            RSAParameters m_RSAKeyInfo = new RSAParameters();

            try
            {
                XmlTextReader reader = new XmlTextReader(PathPrivateKey);

                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.Name == "Modulus")
                        {
                            reader.Read();
                            m_Modulus = reader.Value;
                        }
                        else if (reader.Name == "Exponent")
                        {
                            reader.Read();
                            m_Exponent = reader.Value;
                        }
                        else if (reader.Name == "P")
                        {
                            reader.Read();
                            m_P = reader.Value;
                        }
                        else if (reader.Name == "Q")
                        {
                            reader.Read();
                            m_Q = reader.Value;
                        }
                        else if (reader.Name == "DP")
                        {
                            reader.Read();
                            m_DP = reader.Value;
                        }
                        else if (reader.Name == "DQ")
                        {
                            reader.Read();
                            m_DQ = reader.Value;
                        }
                        else if (reader.Name == "InverseQ")
                        {
                            reader.Read();
                            m_InverseQ = reader.Value;
                        }
                        else if (reader.Name == "D")
                        {
                            reader.Read();
                            m_D = reader.Value;
                        }
                    }
                }

                if (m_Modulus.Equals("") || m_Exponent.Equals(""))
                {
                    throw new Exception("Invalid private key");
                }

                m_RSAKeyInfo.Modulus = Convert.FromBase64String(m_Modulus);
                m_RSAKeyInfo.Exponent = Convert.FromBase64String(m_Exponent);
                m_RSAKeyInfo.P = Convert.FromBase64String(m_P);
                m_RSAKeyInfo.Q = Convert.FromBase64String(m_Q);
                m_RSAKeyInfo.DP = Convert.FromBase64String(m_DP);
                m_RSAKeyInfo.DQ = Convert.FromBase64String(m_DQ);
                m_RSAKeyInfo.InverseQ = Convert.FromBase64String(m_InverseQ);
                m_RSAKeyInfo.D = Convert.FromBase64String(m_D);
                RSAProvider.ImportParameters(m_RSAKeyInfo);
                reader.Close();
                return RSAProvider.ToXmlString(true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string ReadPrivateKey(string p_PathKey)
        {
            string m_Modulus = "";
            string m_Exponent = "";
            string m_P = "";
            string m_Q = "";
            string m_DP = "";
            string m_DQ = "";
            string m_InverseQ = "";
            string m_D = "";

            RSAParameters m_RSAKeyInfo = new RSAParameters();

            try
            {
                XmlTextReader reader = new XmlTextReader(p_PathKey);

                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.Name == "Modulus")
                        {
                            reader.Read();
                            m_Modulus = reader.Value;
                        }
                        else if (reader.Name == "Exponent")
                        {
                            reader.Read();
                            m_Exponent = reader.Value;
                        }
                        else if (reader.Name == "P")
                        {
                            reader.Read();
                            m_P = reader.Value;
                        }
                        else if (reader.Name == "Q")
                        {
                            reader.Read();
                            m_Q = reader.Value;
                        }
                        else if (reader.Name == "DP")
                        {
                            reader.Read();
                            m_DP = reader.Value;
                        }
                        else if (reader.Name == "DQ")
                        {
                            reader.Read();
                            m_DQ = reader.Value;
                        }
                        else if (reader.Name == "InverseQ")
                        {
                            reader.Read();
                            m_InverseQ = reader.Value;
                        }
                        else if (reader.Name == "D")
                        {
                            reader.Read();
                            m_D = reader.Value;
                        }
                    }
                }

                if (m_Modulus.Equals("") || m_Exponent.Equals(""))
                {
                    throw new Exception("Invalid private key");
                }

                m_RSAKeyInfo.Modulus = Convert.FromBase64String(m_Modulus);
                m_RSAKeyInfo.Exponent = Convert.FromBase64String(m_Exponent);
                m_RSAKeyInfo.P = Convert.FromBase64String(m_P);
                m_RSAKeyInfo.Q = Convert.FromBase64String(m_Q);
                m_RSAKeyInfo.DP = Convert.FromBase64String(m_DP);
                m_RSAKeyInfo.DQ = Convert.FromBase64String(m_DQ);
                m_RSAKeyInfo.InverseQ = Convert.FromBase64String(m_InverseQ);
                m_RSAKeyInfo.D = Convert.FromBase64String(m_D);
                RSAProvider.ImportParameters(m_RSAKeyInfo);
                reader.Close();

                return RSAProvider.ToXmlString(true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string ReadPublicKey()
        {
            string m_Modulus = "";
            string m_Exponent = "";
            RSAParameters m_RSAKeyInfo = new RSAParameters();

            try
            {
                XmlTextReader reader = new XmlTextReader(m_PathPublicKey);
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.Name == "Modulus")
                        {
                            reader.Read();
                            m_Modulus = reader.Value;
                        }
                        else if (reader.Name == "Exponent")
                        {
                            reader.Read();
                            m_Exponent = reader.Value;
                        }
                    }
                }
                if (m_Modulus.Equals("") || m_Exponent.Equals(""))
                {
                    throw new Exception("Invalid public key");
                }
                m_RSAKeyInfo.Modulus = Convert.FromBase64String(m_Modulus);
                m_RSAKeyInfo.Exponent = Convert.FromBase64String(m_Exponent);
                RSAProvider.ImportParameters(m_RSAKeyInfo);
                reader.Close();
                return RSAProvider.ToXmlString(false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string ReadPublicKey(string p_PathKey)
        {
            string m_Modulus = "";
            string m_Exponent = "";
            RSAParameters m_RSAKeyInfo = new RSAParameters();

            try
            {
                XmlTextReader reader = new XmlTextReader(p_PathKey);
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.Name == "Modulus")
                        {
                            reader.Read();
                            m_Modulus = reader.Value;
                        }
                        else if (reader.Name == "Exponent")
                        {
                            reader.Read();
                            m_Exponent = reader.Value;
                        }
                    }
                }
                if (m_Modulus.Equals("") || m_Exponent.Equals(""))
                {
                    //throw exception
                    throw new Exception("Invalid public key");
                }
                m_RSAKeyInfo.Modulus = Convert.FromBase64String(m_Modulus);
                m_RSAKeyInfo.Exponent = Convert.FromBase64String(m_Exponent);
                RSAProvider.ImportParameters(m_RSAKeyInfo);
                reader.Close();
                return RSAProvider.ToXmlString(false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public string SignData(string p_Input, string p_PrivateKey)
        //{
        //    RSAProvider.FromXmlString(p_PrivateKey);
        //    byte[] byteKey = null;
        //    byteKey = Encoding.ASCII.GetBytes(p_Input);
        //    byte[] signature = null;
        //    signature = RSAProvider.SignData(byteKey, new SHA1CryptoServiceProvider());
        //    string sSing = Convert.ToBase64String(signature);
        //    return sSing;
        //}

        public string SignData(string p_Input, string p_PrivateKey)
        {
            this.RSAProvider.FromXmlString(p_PrivateKey);
            byte[] bytes = Encoding.ASCII.GetBytes(p_Input);
            byte[] ar = this.RSAProvider.SignData(bytes, new SHA1CryptoServiceProvider());
            return this.Bytes2HexString(ar, ar.Length);
        }

        private string Bytes2HexString(byte[] ar, int len)
        {
            string str3 = "0123456789ABCDEF";
            string str2 = "";
            int num2 = len - 1;
            for (int i = 0; i <= num2; i++)
            {
                str2 = str2 + Convert.ToString(str3[((byte)(ar[i] >> 4)) & 15]) + Convert.ToString(str3[ar[i] & 15]);
            }
            return str2;
        }

        public bool VerifyData(string p_SignInput, string p_OriginInput, string p_PublicKey)
        {
            try
            {
                RSAProvider.FromXmlString(p_PublicKey);
                byte[] byteKey = null;
                byteKey = Encoding.ASCII.GetBytes(p_OriginInput);

                byte[] signature = null;
                signature = Convert.FromBase64String(p_SignInput);
                return RSAProvider.VerifyData(byteKey, new SHA1CryptoServiceProvider(), signature);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public RSAKeyInfo GenerateKey(int p_KeySize)
        {
            RSAProvider = new RSACryptoServiceProvider(p_KeySize);
            RSAKeyInfo o_RSAKey = new RSAKeyInfo(RSAProvider.ToXmlString(true), RSAProvider.ToXmlString(false));
            return o_RSAKey;
        }

        public bool Save(string p_PathKey, int p_KeySize)
        {
            string sFilePublicKey = "netPublicKey.rsa";
            //""
            string sFilePrivateKey = "netPrivateKey.rsa";
            RSAKeyInfo oKey = GenerateKey(p_KeySize);
            bool obSavePub = false;
            bool obSavePri = true;
            obSavePri = SysUtils.SaveTextToFile(oKey.PrivateKey, p_PathKey + "/" + sFilePrivateKey);
            obSavePub = SysUtils.SaveTextToFile(oKey.PublicKey, p_PathKey + "/" + sFilePublicKey);
            if (obSavePri && obSavePub)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public RSAKeyInfo Read(string p_PathPubKey, string p_PathPriKey)
        {
            //Return GetFileContents(p_PathKey)
            RSAKeyInfo oRsa = new RSAKeyInfo();
            oRsa.PublicKey = SysUtils.GetFileContents(p_PathPubKey);
            oRsa.PrivateKey = SysUtils.GetFileContents(p_PathPriKey);
            return oRsa;
        }

        #endregion
    }

    public class RSAKeyInfo
    {

        #region Declare

        private string m_PrivateKey;
        private string m_PublicKey;

        #endregion

        #region Public Property

        public string PrivateKey
        {
            get { return m_PrivateKey; }
            set { m_PrivateKey = value; }
        }

        public string PublicKey
        {
            get { return m_PublicKey; }
            set { m_PublicKey = value; }
        }

        #endregion

        #region Constructors

        public RSAKeyInfo(string p_PrivateKey, string p_PublicKey)
        {
            m_PrivateKey = p_PrivateKey;
            m_PublicKey = p_PublicKey;
        }

        public RSAKeyInfo()
        {
        }

        #endregion
    }

   
}
