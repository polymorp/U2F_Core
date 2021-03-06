﻿using System;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;
using System.Linq;
using U2F.Core.Exceptions;

namespace U2F.Core.Models
{
    public class DeviceRegistration : BaseModel
    {
        public const uint InitialCounterValue = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceRegistration"/> class.
        /// </summary>
        /// <param name="keyHandle">The key handle.</param>
        /// <param name="publicKey">The public key.</param>
        /// <param name="attestationCert">The attestation cert.</param>
        /// <param name="counter">The counter.</param>
        /// <param name="isCompromised"></param>
        /// <exception cref="U2fException">Invalid attestation certificate</exception>
        public DeviceRegistration(byte[] keyHandle, byte[] publicKey, byte[] attestationCert, uint counter, bool isCompromised = false)
        {
            KeyHandle = keyHandle;
            PublicKey = publicKey;
            Counter = counter;
            IsCompromised = isCompromised;

            try
            {
                AttestationCert = attestationCert;
            }
            catch (Exception exception)
            {
                throw new U2fException("Malformed attestation certificate", exception);
            }
        }

        /// <summary>
        /// Gets if the device has been compromised.
        /// </summary>
        /// <value>
        /// If the device has been compromised.
        /// </value>
        public bool IsCompromised { get; private set; }

        /// <summary>
        /// Gets the key handle.
        /// </summary>
        /// <value>
        /// The key handle.
        /// </value>
        public byte[] KeyHandle { get; private set; }

        /// <summary>
        /// Gets the public key.
        /// </summary>
        /// <value>
        /// The public key.
        /// </value>
        public byte[] PublicKey { get; private set; }

        /// <summary>
        /// Gets the attestation cert.
        /// </summary>
        /// <value>
        /// The attestation cert.
        /// </value>
        public byte[] AttestationCert { get; private set; }

        /// <summary>
        /// Usage counter from the device, this should be incremented by 1 every use.
        /// </summary>
        /// <value>
        /// Number of device uses.
        /// </value>
        public uint Counter { get; private set; }

        public X509Certificate GetAttestationCertificate()
        {
            if (AttestationCert == null)
                throw new U2fException("Missing Attestation Certificate.");

            return new X509Certificate(AttestationCert);
        }

        /// <summary>
        /// To the json with out attestation cert.
        /// </summary>
        /// <returns></returns>
        public string ToJsonWithOutAttestionCert()
        {
            return JsonConvert.SerializeObject(new DeviceWithoutCertificate(KeyHandle, PublicKey, Counter, IsCompromised));
        }

        /// <summary>
        /// Checks the and increment counter.
        /// </summary>
        /// <param name="clientCounter">The client counter.</param>
        /// <exception cref="U2fException">Counter value smaller than expected!</exception>
        /// <returns>device counter</returns>
        public uint CheckAndUpdateCounter(uint clientCounter)
        {
            if (clientCounter <= Counter)
            {
                IsCompromised = true;
                throw new U2fException("Counter value smaller than expected!");
            }
            Counter = clientCounter;
            return Counter;
        }

        public override int GetHashCode()
        {
            int hash = PublicKey.Sum(b => b + 31);
            hash += AttestationCert.Sum(b => b + 31);
            hash += KeyHandle.Sum(b => b + 31);

            return hash;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is DeviceRegistration))
                return false;

            DeviceRegistration other = (DeviceRegistration)obj;
            
            return KeyHandle.SequenceEqual(other.KeyHandle)
                   && PublicKey.SequenceEqual(other.PublicKey)
                   && AttestationCert.SequenceEqual(other.AttestationCert)
                   && (IsCompromised == other.IsCompromised);
        }
    }

    internal class DeviceWithoutCertificate
    {
        internal DeviceWithoutCertificate(byte[] keyHandle, byte[] publicKey, uint counter, bool isCompromised)
        {
            KeyHandle = keyHandle;
            PublicKey = publicKey;
            Counter = counter;
            IsCompromised = isCompromised;
        }

        public bool IsCompromised { get; set; }

        public byte[] PublicKey { get; private set; }

        public byte[] KeyHandle { get; private set; }

        public uint Counter { get; private set; }
    }
}
