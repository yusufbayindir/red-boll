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
            "RedBall.HeartStamp",
            "RedBall.Language"
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
            Type localizationType = RequireType("RedBallLocalization");
            MethodInfo setLanguage = RequireMethod(localizationType, "SetLanguage", BindingFlags.Public | BindingFlags.Static);
            MethodInfo badgeSummary = RequireMethod(uiType, "GetLevelBadgeSummary", BindingFlags.Public | BindingFlags.Static);
            MethodInfo completionSummary = RequireMethod(uiType, "GetCompletionBadgeSummary", BindingFlags.Public | BindingFlags.Static);

            setLanguage.Invoke(null, new object[] { "en", false });
            Assert.That(InvokeString(badgeSummary, null, true, false, true), Is.EqualTo("G.T"));
            Assert.That(
                InvokeString(completionSummary, null, true, false, false, true, "damage"),
                Does.Contain("damage"));
        }

        [Test]
        public void LocalizationSupportsRequiredRuntimeLanguages()
        {
            Type localizationType = RequireType("RedBallLocalization");
            MethodInfo isSupported = RequireMethod(localizationType, "IsSupported", BindingFlags.Public | BindingFlags.Static);
            MethodInfo setLanguage = RequireMethod(localizationType, "SetLanguage", BindingFlags.Public | BindingFlags.Static);
            MethodInfo translate = RequireMethod(localizationType, "T", BindingFlags.Public | BindingFlags.Static);
            string[] requiredCodes = { "tr", "en", "es", "fr", "zh-Hans", "hi", "nl", "de", "pt-BR", "ja", "it" };
            string[] nonEnglishCoreKeys = { "button.continue", "button.language", "levelSelect.title", "message.levelComplete", "level.name.15" };

            foreach (string code in requiredCodes)
            {
                Assert.That(InvokeBool(isSupported, null, code), Is.True, code + " should be available in runtime language selector.");
                setLanguage.Invoke(null, new object[] { code, false });
                Assert.That(InvokeString(translate, null, "button.continue"), Is.Not.Empty);
                Assert.That(InvokeString(translate, null, "level.name.14"), Is.Not.Empty);

                if (code != "en")
                {
                    foreach (string key in nonEnglishCoreKeys)
                    {
                        setLanguage.Invoke(null, new object[] { "en", false });
                        string english = InvokeString(translate, null, key);
                        setLanguage.Invoke(null, new object[] { code, false });
                        Assert.That(InvokeString(translate, null, key), Is.Not.EqualTo(english), code + " should localize " + key + " instead of falling back to English.");
                    }
                }
            }
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

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        [TestCase(7)]
        [TestCase(8)]
        [TestCase(9)]
        [TestCase(10)]
        [TestCase(11)]
        [TestCase(12)]
        [TestCase(13)]
        [TestCase(14)]
        public void ExistingLevelExposesRuntimePolishMarkers(int zeroBasedLevelIndex)
        {
            MethodInfo loadLevel = RequireMethod(gameType, "LoadLevelForSmokeTest", BindingFlags.Public | BindingFlags.Instance);

            InvokeString(loadLevel, game, zeroBasedLevelIndex);

            Assert.That(
                CountNamedSpriteObjects("Generated Route Sparkle"),
                Is.GreaterThanOrEqualTo(1),
                "Level " + (zeroBasedLevelIndex + 1) + " should expose at least one Sprint02 route sparkle.");
            Assert.That(
                CountNamedSpriteObjects("Generated Warning Spark"),
                Is.GreaterThanOrEqualTo(1),
                "Level " + (zeroBasedLevelIndex + 1) + " should expose at least one Sprint02 warning sparkle.");
            Assert.That(
                CountNamedSpriteObjects("Generated Sprint04 Parallax Band"),
                Is.GreaterThanOrEqualTo(1),
                "Level " + (zeroBasedLevelIndex + 1) + " should expose the Sprint04 parallax backdrop.");
            Assert.That(
                CountNamedSpriteObjects("Generated Sprint04 Route Arrow"),
                Is.GreaterThanOrEqualTo(1),
                "Level " + (zeroBasedLevelIndex + 1) + " should expose at least one Sprint04 route arrow.");
            Assert.That(
                CountNamedSpriteObjects("Generated Sprint04 Goal Halo"),
                Is.GreaterThanOrEqualTo(1),
                "Level " + (zeroBasedLevelIndex + 1) + " should expose the Sprint04 goal halo.");
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

        private static int CountNamedSpriteObjects(string objectName)
        {
            int count = 0;
            foreach (SpriteRenderer renderer in UnityEngine.Object.FindObjectsByType<SpriteRenderer>(FindObjectsSortMode.None))
            {
                if (renderer != null && renderer.gameObject.name == objectName)
                {
                    count += 1;
                }
            }

            return count;
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
