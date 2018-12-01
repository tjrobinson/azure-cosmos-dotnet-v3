//------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

namespace Microsoft.Azure.Cosmos.Query
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Azure.Cosmos.Internal;
    using Microsoft.Azure.Cosmos.Routing;
    using Newtonsoft.Json;

    // Note: We also return this to client when query execution is disallowed by Gateway
    internal sealed class PartitionedQueryExecutionInfoInternal
    {
        [JsonProperty(Constants.Properties.QueryInfo)]
        public QueryInfo QueryInfo
        {
            get;
            set;
        }

        [JsonProperty(Constants.Properties.QueryRanges)]
        public List<Range<PartitionKeyInternal>> QueryRanges
        {
            get;
            set;
        }
    }
}
