﻿namespace Microsoft.Web.Mvc {
    using System;
    using System.Web.Security;

    // Concrete implementation of IMachineKey that talks to the static MachineKey type

    internal sealed class MachineKeyWrapper : IMachineKey {

        public byte[] Decode(string encodedData, MachineKeyProtection protectionOption) {
            return MachineKey.Decode(encodedData, protectionOption);
        }

        public string Encode(byte[] data, MachineKeyProtection protectionOption) {
            return MachineKey.Encode(data, protectionOption);
        }

    }
}
