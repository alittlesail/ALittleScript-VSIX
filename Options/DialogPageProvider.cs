﻿using System.Runtime.InteropServices;

namespace ALittle
{
    /// <summary>
    /// A provider for custom <see cref="DialogPage" /> implementations.
    /// </summary>
    internal class DialogPageProvider
    {
        [Guid("a154cbb1-0000-39e9-b003-d08dff44e43f")]
        public class General : ALittle.BaseOptionPage<ALittle.GeneralOptions> { }
    }
}
