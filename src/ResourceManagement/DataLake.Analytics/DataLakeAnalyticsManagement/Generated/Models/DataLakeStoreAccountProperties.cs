// 
// Copyright (c) Microsoft and contributors.  All rights reserved.
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//   http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// 
// See the License for the specific language governing permissions and
// limitations under the License.
// 

// Warning: This code was generated by a tool.
// 
// Changes to this file may cause incorrect behavior and will be lost if the
// code is regenerated.

using System;
using System.Linq;

namespace Microsoft.Azure.Management.DataLake.Analytics.Models
{
    /// <summary>
    /// Data Lake Store account properties information.
    /// </summary>
    public partial class DataLakeStoreAccountProperties
    {
        private string _suffix;
        
        /// <summary>
        /// Optional. Gets or sets the optional suffix for the Data Lake Store
        /// account.
        /// </summary>
        public string Suffix
        {
            get { return this._suffix; }
            set { this._suffix = value; }
        }
        
        /// <summary>
        /// Initializes a new instance of the DataLakeStoreAccountProperties
        /// class.
        /// </summary>
        public DataLakeStoreAccountProperties()
        {
        }
    }
}