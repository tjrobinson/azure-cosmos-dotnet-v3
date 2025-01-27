﻿//------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

namespace Microsoft.Azure.Cosmos.Tests
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using Microsoft.Azure.Documents;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class BatchAsyncOperationContextTests
    {
        [TestMethod]
        public void PartitionKeyRangeIdIsSetOnInitialization()
        {
            string expectedPkRangeId = Guid.NewGuid().ToString();
            ItemBatchOperation operation = new ItemBatchOperation(OperationType.Create, 0);
            ItemBatchOperationContext batchAsyncOperationContext = new ItemBatchOperationContext(expectedPkRangeId);
            operation.AttachContext(batchAsyncOperationContext);

            Assert.IsNotNull(batchAsyncOperationContext.Task);
            Assert.AreEqual(batchAsyncOperationContext, operation.Context);
            Assert.AreEqual(expectedPkRangeId, batchAsyncOperationContext.PartitionKeyRangeId);
            Assert.AreEqual(TaskStatus.WaitingForActivation, batchAsyncOperationContext.Task.Status);
        }

        [TestMethod]
        public void TaskIsCreatedOnInitialization()
        {
            ItemBatchOperation operation = new ItemBatchOperation(OperationType.Create, 0);
            ItemBatchOperationContext batchAsyncOperationContext = new ItemBatchOperationContext(string.Empty);
            operation.AttachContext(batchAsyncOperationContext);

            Assert.IsNotNull(batchAsyncOperationContext.Task);
            Assert.AreEqual(batchAsyncOperationContext, operation.Context);
            Assert.AreEqual(TaskStatus.WaitingForActivation, batchAsyncOperationContext.Task.Status);
        }

        [TestMethod]
        public async Task TaskResultIsSetOnCompleteAsync()
        {
            ItemBatchOperation operation = new ItemBatchOperation(OperationType.Create, 0);
            ItemBatchOperationContext batchAsyncOperationContext = new ItemBatchOperationContext(string.Empty);
            operation.AttachContext(batchAsyncOperationContext);

            BatchOperationResult expected = new BatchOperationResult(HttpStatusCode.OK);

            batchAsyncOperationContext.Complete(null, expected);

            Assert.AreEqual(expected, await batchAsyncOperationContext.Task);
            Assert.AreEqual(TaskStatus.RanToCompletion, batchAsyncOperationContext.Task.Status);
        }

        [TestMethod]
        public async Task ExceptionIsSetOnFailAsync()
        {
            Exception failure = new Exception("It failed");
            ItemBatchOperation operation = new ItemBatchOperation(OperationType.Create, 0);
            ItemBatchOperationContext batchAsyncOperationContext = new ItemBatchOperationContext(string.Empty);
            operation.AttachContext(batchAsyncOperationContext);

            batchAsyncOperationContext.Fail(null, failure);

            Exception capturedException = await Assert.ThrowsExceptionAsync<Exception>(() => batchAsyncOperationContext.Task);
            Assert.AreEqual(failure, capturedException);
            Assert.AreEqual(TaskStatus.Faulted, batchAsyncOperationContext.Task.Status);
        }

        [TestMethod]
        public void CannotAttachMoreThanOnce()
        {
            ItemBatchOperation operation = new ItemBatchOperation(OperationType.Create, 0);
            operation.AttachContext(new ItemBatchOperationContext(string.Empty));
            Assert.ThrowsException<InvalidOperationException>(() => operation.AttachContext(new ItemBatchOperationContext(string.Empty)));
        }
    }
}
