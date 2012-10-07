﻿namespace System.Web.Mvc.ExpressionUtil.Test {
    using System;
    using System.Linq.Expressions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class LambdaExpressionFingerprintTest {

        [TestMethod]
        public void Properties() {
            // Arrange
            ExpressionType expectedNodeType = ExpressionType.Lambda;
            Type expectedType = typeof(Action<object>);

            // Act
            LambdaExpressionFingerprint fingerprint = new LambdaExpressionFingerprint(expectedNodeType, expectedType);

            // Assert
            Assert.AreEqual(expectedNodeType, fingerprint.NodeType);
            Assert.AreEqual(expectedType, fingerprint.Type);
        }

        [TestMethod]
        public void Comparison_Equality() {
            // Arrange
            ExpressionType nodeType = ExpressionType.Lambda;
            Type type = typeof(Action<object>);

            // Act
            LambdaExpressionFingerprint fingerprint1 = new LambdaExpressionFingerprint(nodeType, type);
            LambdaExpressionFingerprint fingerprint2 = new LambdaExpressionFingerprint(nodeType, type);

            // Assert
            Assert.AreEqual(fingerprint1, fingerprint2, "Fingerprints should have been equal.");
            Assert.AreEqual(fingerprint1.GetHashCode(), fingerprint2.GetHashCode(), "Fingerprints should have been different.");
        }

        [TestMethod]
        public void Comparison_Inequality_FingerprintType() {
            // Arrange
            ExpressionType nodeType = ExpressionType.Lambda;
            Type type = typeof(Action<object>);

            // Act
            LambdaExpressionFingerprint fingerprint1 = new LambdaExpressionFingerprint(nodeType, type);
            DummyExpressionFingerprint fingerprint2 = new DummyExpressionFingerprint(nodeType, type);

            // Assert
            Assert.AreNotEqual(fingerprint1, fingerprint2, "Fingerprints should not have been equal ('other' is wrong type).");
        }

        [TestMethod]
        public void Comparison_Inequality_NodeType() {
            // Arrange
            ExpressionType nodeType = ExpressionType.Lambda;
            Type type = typeof(Action<object>);

            // Act
            LambdaExpressionFingerprint fingerprint1 = new LambdaExpressionFingerprint(nodeType, type);
            LambdaExpressionFingerprint fingerprint2 = new LambdaExpressionFingerprint(nodeType, typeof(Action));

            // Assert
            Assert.AreNotEqual(fingerprint1, fingerprint2, "Fingerprints should not have been equal (differ by Type).");
        }

    }
}
