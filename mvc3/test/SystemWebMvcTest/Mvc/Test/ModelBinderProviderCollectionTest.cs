﻿namespace System.Web.Mvc.Test {
    using System.Collections.Generic;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class ModelBinderProviderCollectionTest {

        [TestMethod]
        public void GuardClause() {

            //Arrange
            var collection = new ModelBinderProviderCollection();

            //Act & Assert
            ExceptionHelper.ExpectArgumentNullException(
                () => collection.GetBinder(null),
                "modelType"
                );
        }

        [TestMethod]
        public void GetBinderUsesRegisteredProviders() {
            //Arrange
            var testType = typeof(string);
            var expectedBinder = new Mock<IModelBinder>().Object;

            var provider = new Mock<IModelBinderProvider>(MockBehavior.Strict);
            provider.Setup(p => p.GetBinder(testType)).Returns(expectedBinder);
            var collection = new ModelBinderProviderCollection(new[] { provider.Object });

            //Act
            IModelBinder returnedBinder = collection.GetBinder(testType);

            //Assert
            Assert.AreSame(expectedBinder, returnedBinder);
        }

        [TestMethod]
        public void GetBinderReturnsValueFromFirstSuccessfulBinderProvider() {
            //Arrange
            var testType = typeof(string);
            IModelBinder nullModelBinder = null;
            IModelBinder expectedBinder = new Mock<IModelBinder>().Object;
            IModelBinder secondMatchingBinder = new Mock<IModelBinder>().Object;

            var provider1 = new Mock<IModelBinderProvider>();
            provider1.Setup(p => p.GetBinder(testType)).Returns(nullModelBinder);

            var provider2 = new Mock<IModelBinderProvider>(MockBehavior.Strict);
            provider2.Setup(p => p.GetBinder(testType)).Returns(expectedBinder);

            var provider3 = new Mock<IModelBinderProvider>(MockBehavior.Strict);
            provider3.Setup(p => p.GetBinder(testType)).Returns(secondMatchingBinder);

            var collection = new ModelBinderProviderCollection(new[] { provider1.Object, provider2.Object, provider3.Object });

            //Act
            IModelBinder returnedBinder = collection.GetBinder(testType);

            //Assert
            Assert.AreSame(expectedBinder, returnedBinder);
        }

        [TestMethod]
        public void GetBinderReturnsNullWhenNoSuccessfulBinderProviders() {
            //Arrange
            var testType = typeof(string);
            IModelBinder nullModelBinder = null;

            var provider1 = new Mock<IModelBinderProvider>();
            provider1.Setup(p => p.GetBinder(testType)).Returns(nullModelBinder);

            var provider2 = new Mock<IModelBinderProvider>(MockBehavior.Strict);
            provider2.Setup(p => p.GetBinder(testType)).Returns(nullModelBinder);

            var collection = new ModelBinderProviderCollection(new[] { provider1.Object, provider2.Object });

            //Act
            IModelBinder returnedBinder = collection.GetBinder(testType);

            //Assert
            Assert.IsNull(returnedBinder);
        }

        [TestMethod]
        public void GetBinderDelegatesToResolver() {
            //Arrange
            Type modelType = typeof(string);
            IModelBinder expectedBinder = new Mock<IModelBinder>().Object;

            Mock<IModelBinderProvider> locatedProvider = new Mock<IModelBinderProvider>();
            locatedProvider.Setup(p => p.GetBinder(modelType))
                .Returns(expectedBinder);

            Mock<IModelBinderProvider> secondProvider = new Mock<IModelBinderProvider>();
            Resolver<IEnumerable<IModelBinderProvider>> resolver = new Resolver<IEnumerable<IModelBinderProvider>> { Current = new IModelBinderProvider[] { locatedProvider.Object, secondProvider.Object } };

            ModelBinderProviderCollection providers = new ModelBinderProviderCollection(resolver);

            //Act
            IModelBinder returnedBinder = providers.GetBinder(modelType);

            //Assert
            Assert.AreSame(expectedBinder, returnedBinder);
        }
    }
}
