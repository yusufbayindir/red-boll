using System;
using System.Reflection;
using System.Runtime.ExceptionServices;
using NUnit.Framework;
using UnityEngine;

namespace RedBall.Tests.EditMode
{
    public sealed class RedBallSmokeEditModeTests
    {
        private static readonly string[] ProgressKeys =
        {
            "RedBall.UnlockedCount",
            "RedBall.CompletedMask",
            "RedBall.Badges.ClearMask",
            "RedBall.Badges.AllCoinsMask",
            "RedBall.Badges.CleanRunMask",
            "RedBall.Hearts",
            "RedBall.HeartStamp"
        };

        private Type gameType;
        private GameObject gameObject;
        private Component game;

        [SetUp]
        public void SetUp()
        {
            ClearProgressPrefs();
            PlayerPrefs.SetInt("RedBall.UnlockedCount", 15);
            PlayerPrefs.SetInt("RedBall.Hearts", 5);

            gameType = RequireType("RedBallGame");
            gameObject = new GameObject("RedBall Smoke Harness");
            game = gameObject.AddComponent(gameType);

            if (gameObject.transform.Find("Main Camera") == null)
            {
                Invoke(RequireMethod(gameType, "Awake", BindingFlags.NonPublic | BindingFlags.Instance), game);
            }
        }

        [TearDown]
        public void TearDown()
        {
            if (gameObject != null)
            {
                UnityEngine.Object.DestroyImmediate(gameObject);
            }

            ClearProgressPrefs();
        }

        [Test]
        public void RuntimeConstantsExposeFifteenLevels()
        {
            FieldInfo levelCount = gameType.GetField("LevelCount", BindingFlags.Public | BindingFlags.Static);

            Assert.That(levelCount, Is.Not.Null, "RedBallGame.LevelCount must stay public for smoke automation.");
            Assert.That(levelCount.GetValue(null), Is.EqualTo(15));
        }

        [Test]
        public void BadgeMaskHelperReportsTargetLevelBits()
        {
            MethodInfo hasBadge = RequireMethod(gameType, "HasBadgeInMask", BindingFlags.Public | BindingFlags.Static);
            int mask = (1 << 0) | (1 << 13) | (1 << 14);

            Assert.That(InvokeBool(hasBadge, null, mask, 0), Is.True, "Level 1 badge bit should be readable.");
            Assert.That(InvokeBool(hasBadge, null, mask, 13), Is.True, "Level 14 badge bit should be readable.");
            Assert.That(InvokeBool(hasBadge, null, mask, 14), Is.True, "Level 15 badge bit should be readable.");
            Assert.That(InvokeBool(hasBadge, null, mask, 12), Is.False, "Unset badge bits should remain false.");
        }

        [Test]
        public void UiBadgeSummarySurfaceFormatsThreeBadgeStates()
        {
            Type uiType = RequireType("RedBallUi");
            MethodInfo badgeSummary = RequireMethod(uiType, "GetLevelBadgeSummary", BindingFlags.Public | BindingFlags.Static);
            MethodInfo completionSummary = RequireMethod(uiType, "GetCompletionBadgeSummary", BindingFlags.Public | BindingFlags.Static);

            Assert.That(InvokeString(badgeSummary, null, true, false, true), Is.EqualTo("G.T"));
            Assert.That(
                InvokeString(completionSummary, null, true, false, false, true, "damage"),
                Does.Contain("damage"));
        }

        [TestCase(0, "Level 01")]
        [TestCase(13, "Level 14")]
        [TestCase(14, "Level 15")]
        public void TargetLevelsLoadWithoutThrowing(int zeroBasedLevelIndex, string expectedRootName)
        {
            MethodInfo loadLevel = RequireMethod(gameType, "LoadLevelForSmokeTest", BindingFlags.Public | BindingFlags.Instance);
            PropertyInfo levelObjectCount = gameType.GetProperty("LevelObjectCountForSmokeTest", BindingFlags.Public | BindingFlags.Instance);
            PropertyInfo playerTransform = gameType.GetProperty("PlayerTransform", BindingFlags.Public | BindingFlags.Instance);

            string rootName = InvokeString(loadLevel, game, zeroBasedLevelIndex);

            Assert.That(rootName, Does.Contain(expectedRootName));
            Assert.That(levelObjectCount.GetValue(game), Is.GreaterThan(0), "Loading a level should create runtime objects.");
            Assert.That(playerTransform.GetValue(game), Is.Not.Null, "Loading a level should create a player.");
        }

        private static Type RequireType(string typeName)
        {
            Type type = Type.GetType(typeName + ", Assembly-CSharp");
            Assert.That(type, Is.Not.Null, typeName + " was not found in Assembly-CSharp.");
            return type;
        }

        private static MethodInfo RequireMethod(Type type, string methodName, BindingFlags flags)
        {
            MethodInfo method = type.GetMethod(methodName, flags);
            Assert.That(method, Is.Not.Null, type.Name + "." + methodName + " was not found.");
            return method;
        }

        private static bool InvokeBool(MethodInfo method, object target, params object[] args)
        {
            return (bool)Invoke(method, target, args);
        }

        private static string InvokeString(MethodInfo method, object target, params object[] args)
        {
            return (string)Invoke(method, target, args);
        }

        private static object Invoke(MethodInfo method, object target, params object[] args)
        {
            try
            {
                return method.Invoke(target, args);
            }
            catch (TargetInvocationException exception) when (exception.InnerException != null)
            {
                ExceptionDispatchInfo.Capture(exception.InnerException).Throw();
                throw;
            }
        }

        private static void ClearProgressPrefs()
        {
            foreach (string key in ProgressKeys)
            {
                PlayerPrefs.DeleteKey(key);
            }

            PlayerPrefs.Save();
        }
    }
}
