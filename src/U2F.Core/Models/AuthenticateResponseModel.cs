﻿using System;
using System.Linq;

namespace U2F.Core.Models
{
    public class AuthenticateResponseModel : BaseModel
    {
        private readonly ClientData _clientDataRef;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticateResponseModel"/> class.
        /// </summary>
        /// <param name="clientData">The client data.</param>
        /// <param name="signatureData">The signature data.</param>
        /// <param name="keyHandle">The key handle.</param>
        public AuthenticateResponseModel(String clientData, String signatureData, String keyHandle)
        {
            if(String.IsNullOrWhiteSpace(clientData)
                || String.IsNullOrWhiteSpace(signatureData)
                || String.IsNullOrWhiteSpace(keyHandle))
                throw new ArgumentException("Invalid argument(s) were being passed.");

            ClientData = clientData;
            SignatureData = signatureData;
            KeyHandle = keyHandle;
            _clientDataRef = new ClientData(ClientData);
        }

        public ClientData GetClientData()
        {
            return _clientDataRef;
        }

        public String GetRequestId()
        {
            return GetClientData().Challenge;
        }

        /// <summary>
        /// Gets the signature data.
        /// websafe-base64(raw response from U2F device) 
        /// </summary>
        /// <value>
        /// The signature data.
        /// </value>
        public String SignatureData { get; private set; }

        /// <summary>
        /// Gets the Client data.
        /// </summary>
        /// <value>
        /// The Client data.
        /// </value>
        public String ClientData { get; private set; }

        /// <summary>
        ///  keyHandle originally passed
        /// </summary>
        /// <value>
        /// The key handle.
        /// </value>
        public String KeyHandle { get; private set; }

        public override int GetHashCode()
        {
            int hash = ClientData.Sum(c => c + 31);
            hash += SignatureData.Sum(c => c + 31);
            hash += KeyHandle.Sum(c => c + 31);

            return hash;
        }

        public override bool Equals(Object obj)
        {
            if (!(obj is AuthenticateResponseModel))
                return false;
            if (this == obj)
                return true;
            if (GetType() != obj.GetType())
                return false;

            AuthenticateResponseModel other = (AuthenticateResponseModel)obj;

            if (ClientData == null)
            {
                if (other.ClientData != null)
                    return false;
            }
            else if (!ClientData.Equals(other.ClientData))
                return false;

            if (KeyHandle == null)
            {
                if (other.KeyHandle != null)
                    return false;
            }
            else if (!KeyHandle.Equals(other.KeyHandle))
                return false;

            if (SignatureData == null)
            {
                if (other.SignatureData != null)
                    return false;
            }
            else if (!SignatureData.Equals(other.SignatureData))
                return false;

            return true;
        }
    }
}