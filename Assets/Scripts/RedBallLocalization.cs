using System;
using System.Collections.Generic;
using UnityEngine;

public static class RedBallLocalization
{
    public const string PrefKey = "RedBall.Language";

    private static readonly string[] Codes =
    {
        "en",
        "tr",
        "es",
        "fr",
        "zh-Hans",
        "hi",
        "nl",
        "de",
        "pt-BR",
        "ja",
        "it"
    };

    private static readonly Dictionary<string, string> NativeNames = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    {
        { "en", "English" },
        { "tr", "Turkce" },
        { "es", "Espanol" },
        { "fr", "Francais" },
        { "zh-Hans", "简体中文" },
        { "hi", "हिन्दी" },
        { "nl", "Nederlands" },
        { "de", "Deutsch" },
        { "pt-BR", "Portugues BR" },
        { "ja", "日本語" },
        { "it", "Italiano" }
    };

    private static readonly Dictionary<string, Dictionary<string, string>> Tables =
        new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase);

    private static string currentLanguage = "en";

    static RedBallLocalization()
    {
        BuildTables();
    }

    public static string CurrentLanguage => currentLanguage;
    public static IReadOnlyList<string> SupportedLanguages => Codes;

    public static void Initialize()
    {
        string saved = PlayerPrefs.GetString(PrefKey, string.Empty);
        if (!IsSupported(saved))
        {
            saved = MapSystemLanguage(Application.systemLanguage);
        }

        SetLanguage(saved, false);
    }

    public static bool IsSupported(string code)
    {
        if (string.IsNullOrEmpty(code))
        {
            return false;
        }

        for (int i = 0; i < Codes.Length; i++)
        {
            if (string.Equals(Codes[i], code, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }

    public static void SetLanguage(string code, bool save)
    {
        currentLanguage = IsSupported(code) ? NormalizeCode(code) : "en";
        if (save)
        {
            PlayerPrefs.SetString(PrefKey, currentLanguage);
            PlayerPrefs.Save();
        }
    }

    public static string CycleLanguage()
    {
        int index = 0;
        for (int i = 0; i < Codes.Length; i++)
        {
            if (string.Equals(Codes[i], currentLanguage, StringComparison.OrdinalIgnoreCase))
            {
                index = i;
                break;
            }
        }

        SetLanguage(Codes[(index + 1) % Codes.Length], true);
        return currentLanguage;
    }

    public static string T(string key)
    {
        Dictionary<string, string> table;
        string value;
        if (Tables.TryGetValue(currentLanguage, out table) && table.TryGetValue(key, out value))
        {
            return value;
        }

        if (Tables.TryGetValue("en", out table) && table.TryGetValue(key, out value))
        {
            return value;
        }

        return key;
    }

    public static string F(string key, params object[] args)
    {
        return string.Format(T(key), args);
    }

    public static string NativeName(string code)
    {
        string name;
        return NativeNames.TryGetValue(code, out name) ? name : code;
    }

    public static string CurrentLanguageLabel()
    {
        return NativeName(currentLanguage);
    }

    public static string LevelName(int zeroBasedIndex)
    {
        return T("level.name." + Mathf.Clamp(zeroBasedIndex + 1, 1, RedBallGame.LevelCount));
    }

    public static string CleanReason(string reason)
    {
        if (string.IsNullOrEmpty(reason))
        {
            return string.Empty;
        }

        string key = "clean.reason." + reason;
        string localized = T(key);
        return localized == key ? reason : localized;
    }

    public static Font CreateUiFont()
    {
        string[] names =
        {
            "SF Pro Rounded",
            "SF Pro Display",
            "Arial Unicode MS",
            "Noto Sans",
            "Noto Sans CJK SC",
            "Noto Sans Devanagari",
            "PingFang SC",
            "Hiragino Sans",
            "Yu Gothic",
            "Meiryo",
            "Microsoft YaHei",
            "Nirmala UI",
            "Mangal",
            "Segoe UI",
            "Arial"
        };

        for (int i = 0; i < names.Length; i++)
        {
            try
            {
                Font font = Font.CreateDynamicFontFromOSFont(names[i], 18);
                if (font != null)
                {
                    return font;
                }
            }
            catch (Exception)
            {
                // Some Unity/player targets throw when a family is not installed.
            }
        }

        Font fallback = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        return fallback != null ? fallback : Resources.GetBuiltinResource<Font>("Arial.ttf");
    }

    private static string MapSystemLanguage(SystemLanguage language)
    {
        string value = language.ToString();
        if (value == "Turkish") return "tr";
        if (value == "Spanish") return "es";
        if (value == "French") return "fr";
        if (value == "Chinese" || value == "ChineseSimplified") return "zh-Hans";
        if (value == "Hindi") return "hi";
        if (value == "Dutch") return "nl";
        if (value == "German") return "de";
        if (value == "Portuguese") return "pt-BR";
        if (value == "Japanese") return "ja";
        if (value == "Italian") return "it";
        return "en";
    }

    private static string NormalizeCode(string code)
    {
        for (int i = 0; i < Codes.Length; i++)
        {
            if (string.Equals(Codes[i], code, StringComparison.OrdinalIgnoreCase))
            {
                return Codes[i];
            }
        }

        return "en";
    }

    private static void BuildTables()
    {
        var en = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "menu.banner", "SPRINT 03 MODERN UI" },
            { "menu.title", "RED BALL\nMASTERY" },
            { "menu.subtitle", "15 hand-built levels with mastery badges" },
            { "menu.feature.lifts", "Lift timing" },
            { "menu.feature.crumble", "Crumbling tiles" },
            { "menu.feature.badges", "Clear / Coins / Clean" },
            { "button.continue", "Continue" },
            { "button.levels", "Levels" },
            { "button.back", "Back" },
            { "button.language", "Language: {0}" },
            { "levelSelect.title", "Level Select" },
            { "levelSelect.subtitle", "Choose a route, chase badges, preview late-game mechanics." },
            { "levelSelect.legend", "Badge marks: Clear, All Coins, Clean Run. Featured cards preview lifts and crumble routes." },
            { "level.tag.lift", "LIFT" },
            { "level.tag.crumble", "CRUMBLE" },
            { "hud.health", "Health" },
            { "hud.score", "Score" },
            { "hud.coin", "Coin" },
            { "hud.level", "Level" },
            { "life.full", "Full" },
            { "life.next", "Next" },
            { "time.hourShort", "1h" },
            { "time.minShort", "{0}m" },
            { "status.noHearts", "No hearts. Next heart: {0}" },
            { "status.locked", "This level is locked. Clear the previous level first." },
            { "status.heartsExhausted", "Out of hearts. One heart returns every hour." },
            { "message.noHearts", "No hearts left. One heart returns in 1 hour." },
            { "message.damage", "Careful! You lost a heart and returned to checkpoint." },
            { "message.checkpoint", "Checkpoint!" },
            { "message.badgesPrefix", "Badges:" },
            { "message.levelComplete", "Level complete! {0}" },
            { "message.gameComplete", "Congratulations! All levels complete. {0}" },
            { "level.state.locked", "Locked" },
            { "level.state.completed", "Clear" },
            { "level.state.continue", "Continue" },
            { "level.state.current", "Playing" },
            { "completion.clear.yes", "Clear earned" },
            { "completion.clear.no", "Clear missing" },
            { "completion.coins.none", "No coins" },
            { "completion.coins.yes", "All coins" },
            { "completion.coins.no", "Coins missing" },
            { "completion.clean.yes", "Clean run" },
            { "completion.clean.no", "Clean missing" },
            { "clean.reason.damage", "damage" },
            { "clean.reason.restart", "restart" },
            { "level.feature.14", "Lifts" },
            { "level.feature.15", "Crumble" },
            { "level.name.1", "Level 1: Rolling Lesson" },
            { "level.name.2", "Level 2: First Hazard" },
            { "level.name.3", "Level 3: Bounce Lesson" },
            { "level.name.4", "Level 4: Thin Steps" },
            { "level.name.5", "Level 5: Patrol Route" },
            { "level.name.6", "Level 6: Moving Bridge" },
            { "level.name.7", "Level 7: Bounce Stairway" },
            { "level.name.8", "Level 8: Rhythm Bridge" },
            { "level.name.9", "Level 9: High Line" },
            { "level.name.10", "Level 10: Descent Exit" },
            { "level.name.11", "Level 11: Patrol Gate" },
            { "level.name.12", "Level 12: Sharp Bounce" },
            { "level.name.13", "Level 13: Final Run" },
            { "level.name.14", "Level 14: Lift Garden" },
            { "level.name.15", "Level 15: Broken Stones" }
        };

        Tables["en"] = en;
        AddLanguage("tr", en, new Dictionary<string, string> {
            { "menu.banner", "SPRINT 03 MODERN ARAYUZ" }, { "menu.title", "RED BALL\nUSTALIK" }, { "menu.subtitle", "Rozetli 15 ozenli bolum" },
            { "menu.feature.lifts", "Asansor zamanlamasi" }, { "menu.feature.crumble", "Kirilan taslar" }, { "menu.feature.badges", "Gecis / Coin / Temiz" },
            { "button.continue", "Devam Et" }, { "button.levels", "Bolumler" }, { "button.back", "Geri" }, { "button.language", "Dil: {0}" },
            { "levelSelect.title", "Bolum Sec" }, { "levelSelect.subtitle", "Rotayi sec, rozetleri kovala, son mekanikleri onizle." }, { "levelSelect.legend", "Rozetler: Gecis, Tum Coin, Temiz Kosu. One cikan kartlar asansor ve kirilan rotalari gosterir." },
            { "level.tag.lift", "ASANSOR" }, { "level.tag.crumble", "KIRILAN" }, { "hud.health", "Can" }, { "hud.score", "Skor" }, { "hud.coin", "Coin" }, { "hud.level", "Bolum" },
            { "life.full", "Dolu" }, { "life.next", "Sonraki" }, { "time.hourShort", "1 sa" }, { "time.minShort", "{0} dk" },
            { "status.noHearts", "Kalp yok. Sonraki kalp: {0}" }, { "status.locked", "Bu bolum kilitli. Once onceki bolumu gec." }, { "status.heartsExhausted", "Kalpler bitti. Her saat 1 kalp geri gelir." },
            { "message.noHearts", "Kalp bitti! 1 saat sonra 1 kalp gelir." }, { "message.damage", "Dikkat! Kalp gitti, checkpoint'e geri donuldu." }, { "message.badgesPrefix", "Rozetler:" },
            { "message.levelComplete", "Bolum tamam! {0}" }, { "message.gameComplete", "Tebrikler! Tum bolumler tamamlandi. {0}" },
            { "level.state.locked", "Kilitli" }, { "level.state.completed", "Gecti" }, { "level.state.continue", "Devam" }, { "level.state.current", "Oynuyor" },
            { "completion.clear.yes", "Gecis var" }, { "completion.clear.no", "Gecis yok" }, { "completion.coins.none", "Coin yok" }, { "completion.coins.yes", "Tum coin var" }, { "completion.coins.no", "Coin eksik" }, { "completion.clean.yes", "Temiz var" }, { "completion.clean.no", "Temiz yok" },
            { "clean.reason.damage", "hasar" }, { "clean.reason.restart", "yeniden baslatma" }, { "level.feature.14", "Asansor" }, { "level.feature.15", "Kirilan" },
            { "level.name.1", "Bolum 1: Yuvarlanma Dersi" }, { "level.name.2", "Bolum 2: Ilk Tehlike" }, { "level.name.3", "Bolum 3: Sekme Dersi" }, { "level.name.4", "Bolum 4: Ince Adim" }, { "level.name.5", "Bolum 5: Devriye Yolu" }, { "level.name.6", "Bolum 6: Hareketli Kopru" }, { "level.name.7", "Bolum 7: Sekme Merdiveni" }, { "level.name.8", "Bolum 8: Ritim Koprusu" }, { "level.name.9", "Bolum 9: Yuksek Hat" }, { "level.name.10", "Bolum 10: Inis Cikisi" }, { "level.name.11", "Bolum 11: Devriye Kapisi" }, { "level.name.12", "Bolum 12: Keskin Sekme" }, { "level.name.13", "Bolum 13: Son Kosu" }, { "level.name.14", "Bolum 14: Asansor Bahcesi" }, { "level.name.15", "Bolum 15: Kirik Taslar" }
        });

        AddRomanceAndGerman(en);
        AddAsianAndIndic(en);
    }

    private static void AddLanguage(string code, Dictionary<string, string> fallback, Dictionary<string, string> overlay)
    {
        var table = new Dictionary<string, string>(fallback, StringComparer.OrdinalIgnoreCase);
        foreach (KeyValuePair<string, string> pair in overlay)
        {
            table[pair.Key] = pair.Value;
        }

        Tables[code] = table;
    }

    private static void AddRomanceAndGerman(Dictionary<string, string> en)
    {
        AddLanguage("es", en, new Dictionary<string, string> {
            { "menu.banner", "SPRINT 03 UI MODERNA" }, { "menu.title", "RED BALL\nMAESTRIA" }, { "menu.subtitle", "15 niveles hechos a mano con insignias" },
            { "menu.feature.lifts", "Ritmo de elevadores" }, { "menu.feature.crumble", "Losas que se rompen" }, { "menu.feature.badges", "Pasar / Monedas / Limpio" },
            { "button.continue", "Continuar" }, { "button.levels", "Niveles" }, { "button.back", "Atras" }, { "button.language", "Idioma: {0}" },
            { "levelSelect.title", "Seleccion de niveles" }, { "levelSelect.subtitle", "Elige ruta, busca insignias y prueba mecanicas finales." }, { "levelSelect.legend", "Insignias: Pasar, Todas las monedas, Carrera limpia. Las cartas destacadas muestran elevadores y rutas frágiles." },
            { "level.tag.lift", "ELEVADOR" }, { "level.tag.crumble", "ROMPE" }, { "hud.health", "Vida" }, { "hud.score", "Puntos" }, { "hud.coin", "Monedas" }, { "hud.level", "Nivel" },
            { "life.full", "Lleno" }, { "life.next", "Siguiente" }, { "time.hourShort", "1 h" }, { "time.minShort", "{0} min" },
            { "status.noHearts", "Sin corazones. Siguiente: {0}" }, { "status.locked", "Este nivel esta bloqueado. Supera el anterior." }, { "status.heartsExhausted", "Sin corazones. Recuperas uno cada hora." },
            { "message.noHearts", "No quedan corazones. Recuperas uno en 1 hora." }, { "message.damage", "Cuidado! Perdiste una vida y volviste al checkpoint." }, { "message.checkpoint", "Checkpoint!" }, { "message.badgesPrefix", "Insignias:" },
            { "message.levelComplete", "Nivel completado! {0}" }, { "message.gameComplete", "Felicidades! Completaste todos los niveles. {0}" },
            { "level.state.locked", "Bloqueado" }, { "level.state.completed", "Listo" }, { "level.state.continue", "Continuar" }, { "level.state.current", "Jugando" },
            { "completion.clear.yes", "Pasado" }, { "completion.clear.no", "Falta pasar" }, { "completion.coins.none", "Sin monedas" }, { "completion.coins.yes", "Todas las monedas" }, { "completion.coins.no", "Faltan monedas" }, { "completion.clean.yes", "Carrera limpia" }, { "completion.clean.no", "Limpia faltante" },
            { "clean.reason.damage", "dano" }, { "clean.reason.restart", "reinicio" }, { "level.feature.14", "Elevadores" }, { "level.feature.15", "Se rompe" },
            { "level.name.1", "Nivel 1: Leccion de rodar" }, { "level.name.2", "Nivel 2: Primer peligro" }, { "level.name.3", "Nivel 3: Leccion de rebote" }, { "level.name.4", "Nivel 4: Pasos finos" }, { "level.name.5", "Nivel 5: Ruta de patrulla" }, { "level.name.6", "Nivel 6: Puente movil" }, { "level.name.7", "Nivel 7: Escalera de rebote" }, { "level.name.8", "Nivel 8: Puente ritmico" }, { "level.name.9", "Nivel 9: Linea alta" }, { "level.name.10", "Nivel 10: Salida en bajada" }, { "level.name.11", "Nivel 11: Puerta de patrulla" }, { "level.name.12", "Nivel 12: Rebote preciso" }, { "level.name.13", "Nivel 13: Carrera final" }, { "level.name.14", "Nivel 14: Jardin de elevadores" }, { "level.name.15", "Nivel 15: Piedras rotas" }
        });
        AddLanguage("fr", en, new Dictionary<string, string> {
            { "menu.banner", "SPRINT 03 UI MODERNE" }, { "menu.title", "RED BALL\nMAITRISE" }, { "menu.subtitle", "15 niveaux faits main avec badges" },
            { "menu.feature.lifts", "Timing des ascenseurs" }, { "menu.feature.crumble", "Dalles fragiles" }, { "menu.feature.badges", "Terminer / Pieces / Propre" },
            { "button.continue", "Continuer" }, { "button.levels", "Niveaux" }, { "button.back", "Retour" }, { "button.language", "Langue: {0}" },
            { "levelSelect.title", "Choix du niveau" }, { "levelSelect.subtitle", "Choisis une route, vise les badges et teste les mecaniques finales." }, { "levelSelect.legend", "Badges: Termine, Toutes les pieces, Course propre. Les cartes en avant montrent ascenseurs et routes fragiles." },
            { "level.tag.lift", "ASCENSEUR" }, { "level.tag.crumble", "FRAGILE" }, { "hud.health", "Vie" }, { "hud.score", "Score" }, { "hud.coin", "Pieces" }, { "hud.level", "Niveau" },
            { "life.full", "Plein" }, { "life.next", "Suivant" }, { "time.hourShort", "1 h" }, { "time.minShort", "{0} min" },
            { "status.noHearts", "Plus de coeurs. Suivant: {0}" }, { "status.locked", "Ce niveau est verrouille. Termine le precedent." }, { "status.heartsExhausted", "Plus de coeurs. Un coeur revient chaque heure." },
            { "message.noHearts", "Plus de coeurs. Un coeur revient dans 1 heure." }, { "message.damage", "Attention! Tu perds une vie et reviens au checkpoint." }, { "message.checkpoint", "Checkpoint!" }, { "message.badgesPrefix", "Badges:" },
            { "message.levelComplete", "Niveau termine! {0}" }, { "message.gameComplete", "Bravo! Tous les niveaux sont termines. {0}" },
            { "level.state.locked", "Verrouille" }, { "level.state.completed", "Reussi" }, { "level.state.continue", "Continuer" }, { "level.state.current", "En cours" },
            { "completion.clear.yes", "Termine" }, { "completion.clear.no", "Termine manquant" }, { "completion.coins.none", "Aucune piece" }, { "completion.coins.yes", "Toutes les pieces" }, { "completion.coins.no", "Pieces manquantes" }, { "completion.clean.yes", "Course propre" }, { "completion.clean.no", "Propre manquant" },
            { "clean.reason.damage", "degat" }, { "clean.reason.restart", "redemarrage" }, { "level.feature.14", "Ascenseurs" }, { "level.feature.15", "Fragile" },
            { "level.name.1", "Niveau 1: Lecon de roulade" }, { "level.name.2", "Niveau 2: Premier danger" }, { "level.name.3", "Niveau 3: Lecon de rebond" }, { "level.name.4", "Niveau 4: Pas fins" }, { "level.name.5", "Niveau 5: Route de patrouille" }, { "level.name.6", "Niveau 6: Pont mobile" }, { "level.name.7", "Niveau 7: Escalier rebond" }, { "level.name.8", "Niveau 8: Pont rythme" }, { "level.name.9", "Niveau 9: Ligne haute" }, { "level.name.10", "Niveau 10: Sortie descendante" }, { "level.name.11", "Niveau 11: Porte de patrouille" }, { "level.name.12", "Niveau 12: Rebond net" }, { "level.name.13", "Niveau 13: Course finale" }, { "level.name.14", "Niveau 14: Jardin des ascenseurs" }, { "level.name.15", "Niveau 15: Pierres brisees" }
        });
        AddLanguage("nl", en, new Dictionary<string, string> {
            { "menu.banner", "SPRINT 03 MODERNE UI" }, { "menu.title", "RED BALL\nMASTERY" }, { "menu.subtitle", "15 handgemaakte levels met badges" },
            { "menu.feature.lifts", "Lifttiming" }, { "menu.feature.crumble", "Breekbare tegels" }, { "menu.feature.badges", "Uitspelen / Munten / Clean" },
            { "button.continue", "Verder" }, { "button.levels", "Levels" }, { "button.back", "Terug" }, { "button.language", "Taal: {0}" },
            { "levelSelect.title", "Levelkeuze" }, { "levelSelect.subtitle", "Kies een route, jaag op badges en bekijk late mechanics." }, { "levelSelect.legend", "Badges: Uitgespeeld, Alle munten, Clean run. Uitgelichte kaarten tonen liften en breekroutes." },
            { "level.tag.lift", "LIFT" }, { "level.tag.crumble", "BREEKT" }, { "hud.health", "Leven" }, { "hud.score", "Score" }, { "hud.coin", "Munten" }, { "hud.level", "Level" },
            { "life.full", "Vol" }, { "life.next", "Volgende" }, { "time.hourShort", "1 u" }, { "time.minShort", "{0} min" },
            { "status.noHearts", "Geen harten. Volgende: {0}" }, { "status.locked", "Dit level is op slot. Haal eerst het vorige level." }, { "status.heartsExhausted", "Geen harten meer. Elk uur komt er een terug." },
            { "message.noHearts", "Geen harten over. Over 1 uur komt er een terug." }, { "message.damage", "Voorzichtig! Je verloor een hart en keerde terug naar checkpoint." }, { "message.checkpoint", "Checkpoint!" }, { "message.badgesPrefix", "Badges:" },
            { "message.levelComplete", "Level voltooid! {0}" }, { "message.gameComplete", "Gefeliciteerd! Alle levels voltooid. {0}" },
            { "level.state.locked", "Op slot" }, { "level.state.completed", "Gehaald" }, { "level.state.continue", "Verder" }, { "level.state.current", "Bezig" },
            { "completion.clear.yes", "Gehaald" }, { "completion.clear.no", "Clear mist" }, { "completion.coins.none", "Geen munten" }, { "completion.coins.yes", "Alle munten" }, { "completion.coins.no", "Munten missen" }, { "completion.clean.yes", "Clean run" }, { "completion.clean.no", "Clean mist" },
            { "clean.reason.damage", "schade" }, { "clean.reason.restart", "herstart" }, { "level.feature.14", "Liften" }, { "level.feature.15", "Breekt" },
            { "level.name.1", "Level 1: Rolvedles" }, { "level.name.2", "Level 2: Eerste gevaar" }, { "level.name.3", "Level 3: Stuiterles" }, { "level.name.4", "Level 4: Smalle stappen" }, { "level.name.5", "Level 5: Patrouilleroute" }, { "level.name.6", "Level 6: Bewegende brug" }, { "level.name.7", "Level 7: Stuitertrap" }, { "level.name.8", "Level 8: Ritmebrug" }, { "level.name.9", "Level 9: Hoge lijn" }, { "level.name.10", "Level 10: Afdaling" }, { "level.name.11", "Level 11: Patrouillepoort" }, { "level.name.12", "Level 12: Scherpe stuiter" }, { "level.name.13", "Level 13: Laatste run" }, { "level.name.14", "Level 14: Lifttuin" }, { "level.name.15", "Level 15: Gebroken stenen" }
        });
        AddLanguage("de", en, new Dictionary<string, string> {
            { "menu.banner", "SPRINT 03 MODERNE UI" }, { "menu.title", "RED BALL\nMEISTERSCHAFT" }, { "menu.subtitle", "15 handgebaute Level mit Abzeichen" },
            { "menu.feature.lifts", "Lift-Timing" }, { "menu.feature.crumble", "Bruckelnde Platten" }, { "menu.feature.badges", "Clear / Munzen / Sauber" },
            { "button.continue", "Weiter" }, { "button.levels", "Level" }, { "button.back", "Zuruck" }, { "button.language", "Sprache: {0}" },
            { "levelSelect.title", "Levelauswahl" }, { "levelSelect.subtitle", "Wahle eine Route, jage Abzeichen und teste spate Mechaniken." }, { "levelSelect.legend", "Abzeichen: Geschafft, Alle Munzen, Sauberer Lauf. Karten zeigen Lifte und bruchige Routen." },
            { "level.tag.lift", "LIFT" }, { "level.tag.crumble", "BRUCH" }, { "hud.health", "Leben" }, { "hud.score", "Punkte" }, { "hud.coin", "Munzen" }, { "hud.level", "Level" },
            { "life.full", "Voll" }, { "life.next", "Nachste" }, { "time.hourShort", "1 Std" }, { "time.minShort", "{0} Min" },
            { "status.noHearts", "Keine Herzen. Nachstes: {0}" }, { "status.locked", "Dieses Level ist gesperrt. Schaffe zuerst das vorherige." }, { "status.heartsExhausted", "Keine Herzen mehr. Jede Stunde kommt eines zuruck." },
            { "message.noHearts", "Keine Herzen ubrig. In 1 Stunde kommt eines zuruck." }, { "message.damage", "Vorsicht! Du hast ein Herz verloren und bist am Checkpoint." }, { "message.checkpoint", "Checkpoint!" }, { "message.badgesPrefix", "Abzeichen:" },
            { "message.levelComplete", "Level geschafft! {0}" }, { "message.gameComplete", "Gluckwunsch! Alle Level geschafft. {0}" },
            { "level.state.locked", "Gesperrt" }, { "level.state.completed", "Geschafft" }, { "level.state.continue", "Weiter" }, { "level.state.current", "Aktiv" },
            { "completion.clear.yes", "Geschafft" }, { "completion.clear.no", "Clear fehlt" }, { "completion.coins.none", "Keine Munzen" }, { "completion.coins.yes", "Alle Munzen" }, { "completion.coins.no", "Munzen fehlen" }, { "completion.clean.yes", "Sauberer Lauf" }, { "completion.clean.no", "Sauber fehlt" },
            { "clean.reason.damage", "Schaden" }, { "clean.reason.restart", "Neustart" }, { "level.feature.14", "Lifte" }, { "level.feature.15", "Bruch" },
            { "level.name.1", "Level 1: Roll-Lektion" }, { "level.name.2", "Level 2: Erste Gefahr" }, { "level.name.3", "Level 3: Sprung-Lektion" }, { "level.name.4", "Level 4: Schmale Schritte" }, { "level.name.5", "Level 5: Patrouillenroute" }, { "level.name.6", "Level 6: Bewegliche Brucke" }, { "level.name.7", "Level 7: Sprungtreppe" }, { "level.name.8", "Level 8: Rhythmusbrucke" }, { "level.name.9", "Level 9: Hohe Linie" }, { "level.name.10", "Level 10: Abstieg" }, { "level.name.11", "Level 11: Patrouillentor" }, { "level.name.12", "Level 12: Scharfer Sprung" }, { "level.name.13", "Level 13: Finaler Lauf" }, { "level.name.14", "Level 14: Liftgarten" }, { "level.name.15", "Level 15: Gebrochene Steine" }
        });
        AddLanguage("pt-BR", en, new Dictionary<string, string> {
            { "menu.banner", "SPRINT 03 UI MODERNA" }, { "menu.title", "RED BALL\nMAESTRIA" }, { "menu.subtitle", "15 fases feitas a mao com emblemas" },
            { "menu.feature.lifts", "Ritmo dos elevadores" }, { "menu.feature.crumble", "Blocos quebraveis" }, { "menu.feature.badges", "Concluir / Moedas / Limpo" },
            { "button.continue", "Continuar" }, { "button.levels", "Fases" }, { "button.back", "Voltar" }, { "button.language", "Idioma: {0}" },
            { "levelSelect.title", "Selecionar fase" }, { "levelSelect.subtitle", "Escolha uma rota, busque emblemas e veja mecanicas finais." }, { "levelSelect.legend", "Emblemas: Concluir, Todas as moedas, Corrida limpa. Cartas mostram elevadores e rotas quebraveis." },
            { "level.tag.lift", "ELEVADOR" }, { "level.tag.crumble", "QUEBRA" }, { "hud.health", "Vida" }, { "hud.score", "Pontos" }, { "hud.coin", "Moedas" }, { "hud.level", "Fase" },
            { "life.full", "Cheio" }, { "life.next", "Proximo" }, { "time.hourShort", "1 h" }, { "time.minShort", "{0} min" },
            { "status.noHearts", "Sem coracoes. Proximo: {0}" }, { "status.locked", "Esta fase esta bloqueada. Conclua a anterior." }, { "status.heartsExhausted", "Sem coracoes. Um coracao volta a cada hora." },
            { "message.noHearts", "Sem coracoes. Um coracao volta em 1 hora." }, { "message.damage", "Cuidado! Voce perdeu um coracao e voltou ao checkpoint." }, { "message.checkpoint", "Checkpoint!" }, { "message.badgesPrefix", "Emblemas:" },
            { "message.levelComplete", "Fase concluida! {0}" }, { "message.gameComplete", "Parabens! Todas as fases concluidas. {0}" },
            { "level.state.locked", "Bloqueada" }, { "level.state.completed", "Concluida" }, { "level.state.continue", "Continuar" }, { "level.state.current", "Jogando" },
            { "completion.clear.yes", "Concluida" }, { "completion.clear.no", "Falta concluir" }, { "completion.coins.none", "Sem moedas" }, { "completion.coins.yes", "Todas as moedas" }, { "completion.coins.no", "Faltam moedas" }, { "completion.clean.yes", "Corrida limpa" }, { "completion.clean.no", "Limpa faltando" },
            { "clean.reason.damage", "dano" }, { "clean.reason.restart", "reinicio" }, { "level.feature.14", "Elevadores" }, { "level.feature.15", "Quebra" },
            { "level.name.1", "Fase 1: Aula de rolar" }, { "level.name.2", "Fase 2: Primeiro perigo" }, { "level.name.3", "Fase 3: Aula de quique" }, { "level.name.4", "Fase 4: Passos finos" }, { "level.name.5", "Fase 5: Rota de patrulha" }, { "level.name.6", "Fase 6: Ponte movel" }, { "level.name.7", "Fase 7: Escada de quique" }, { "level.name.8", "Fase 8: Ponte ritmica" }, { "level.name.9", "Fase 9: Linha alta" }, { "level.name.10", "Fase 10: Saida descendo" }, { "level.name.11", "Fase 11: Portao de patrulha" }, { "level.name.12", "Fase 12: Quique preciso" }, { "level.name.13", "Fase 13: Corrida final" }, { "level.name.14", "Fase 14: Jardim dos elevadores" }, { "level.name.15", "Fase 15: Pedras quebradas" }
        });
        AddLanguage("it", en, new Dictionary<string, string> {
            { "menu.banner", "SPRINT 03 UI MODERNA" }, { "menu.title", "RED BALL\nMAESTRIA" }, { "menu.subtitle", "15 livelli fatti a mano con badge" },
            { "menu.feature.lifts", "Timing ascensori" }, { "menu.feature.crumble", "Piastre fragili" }, { "menu.feature.badges", "Completa / Monete / Pulito" },
            { "button.continue", "Continua" }, { "button.levels", "Livelli" }, { "button.back", "Indietro" }, { "button.language", "Lingua: {0}" },
            { "levelSelect.title", "Seleziona livello" }, { "levelSelect.subtitle", "Scegli una rotta, cerca badge e prova meccaniche finali." }, { "levelSelect.legend", "Badge: Completato, Tutte le monete, Run pulita. Le carte mostrano ascensori e percorsi fragili." },
            { "level.tag.lift", "ASCENSORE" }, { "level.tag.crumble", "FRAGILE" }, { "hud.health", "Vita" }, { "hud.score", "Punti" }, { "hud.coin", "Monete" }, { "hud.level", "Livello" },
            { "life.full", "Pieno" }, { "life.next", "Prossimo" }, { "time.hourShort", "1 h" }, { "time.minShort", "{0} min" },
            { "status.noHearts", "Nessun cuore. Prossimo: {0}" }, { "status.locked", "Questo livello e bloccato. Completa il precedente." }, { "status.heartsExhausted", "Cuori finiti. Un cuore torna ogni ora." },
            { "message.noHearts", "Non restano cuori. Un cuore torna in 1 ora." }, { "message.damage", "Attento! Hai perso un cuore e sei tornato al checkpoint." }, { "message.checkpoint", "Checkpoint!" }, { "message.badgesPrefix", "Badge:" },
            { "message.levelComplete", "Livello completato! {0}" }, { "message.gameComplete", "Complimenti! Tutti i livelli completati. {0}" },
            { "level.state.locked", "Bloccato" }, { "level.state.completed", "Completato" }, { "level.state.continue", "Continua" }, { "level.state.current", "In gioco" },
            { "completion.clear.yes", "Completato" }, { "completion.clear.no", "Completa mancante" }, { "completion.coins.none", "Nessuna moneta" }, { "completion.coins.yes", "Tutte le monete" }, { "completion.coins.no", "Monete mancanti" }, { "completion.clean.yes", "Run pulita" }, { "completion.clean.no", "Pulita mancante" },
            { "clean.reason.damage", "danno" }, { "clean.reason.restart", "riavvio" }, { "level.feature.14", "Ascensori" }, { "level.feature.15", "Crollo" },
            { "level.name.1", "Livello 1: Lezione di rotolamento" }, { "level.name.2", "Livello 2: Primo pericolo" }, { "level.name.3", "Livello 3: Lezione di rimbalzo" }, { "level.name.4", "Livello 4: Passi sottili" }, { "level.name.5", "Livello 5: Rotta pattuglia" }, { "level.name.6", "Livello 6: Ponte mobile" }, { "level.name.7", "Livello 7: Scala rimbalzo" }, { "level.name.8", "Livello 8: Ponte ritmo" }, { "level.name.9", "Livello 9: Linea alta" }, { "level.name.10", "Livello 10: Uscita in discesa" }, { "level.name.11", "Livello 11: Cancello pattuglia" }, { "level.name.12", "Livello 12: Rimbalzo netto" }, { "level.name.13", "Livello 13: Corsa finale" }, { "level.name.14", "Livello 14: Giardino ascensori" }, { "level.name.15", "Livello 15: Pietre rotte" }
        });
    }

    private static void AddAsianAndIndic(Dictionary<string, string> en)
    {
        AddLanguage("zh-Hans", en, new Dictionary<string, string> {
            { "menu.banner", "SPRINT 03 现代界面" }, { "menu.title", "红球\n精通挑战" }, { "menu.subtitle", "15 个手工关卡与精通徽章" },
            { "menu.feature.lifts", "升降台节奏" }, { "menu.feature.crumble", "碎裂地块" }, { "menu.feature.badges", "通关 / 金币 / 无伤" },
            { "button.continue", "继续" }, { "button.levels", "关卡" }, { "button.back", "返回" }, { "button.language", "语言：{0}" },
            { "levelSelect.title", "选择关卡" }, { "levelSelect.subtitle", "选择路线，收集徽章，预览后期机制。" }, { "levelSelect.legend", "徽章：通关、全部金币、无伤路线。高亮卡片展示升降台与碎裂路线。" },
            { "level.tag.lift", "升降" }, { "level.tag.crumble", "碎裂" }, { "hud.health", "生命" }, { "hud.score", "分数" }, { "hud.coin", "金币" }, { "hud.level", "关卡" },
            { "life.full", "已满" }, { "life.next", "下一个" }, { "time.hourShort", "1小时" }, { "time.minShort", "{0}分" },
            { "status.noHearts", "没有爱心。下一个：{0}" }, { "status.locked", "此关卡已锁定。请先通关上一关。" }, { "status.heartsExhausted", "爱心用完了。每小时恢复 1 个。" },
            { "message.noHearts", "没有爱心了。1 小时后恢复 1 个。" }, { "message.damage", "小心！失去一颗心并回到检查点。" }, { "message.checkpoint", "检查点！" }, { "message.badgesPrefix", "徽章：" },
            { "message.levelComplete", "关卡完成！{0}" }, { "message.gameComplete", "恭喜！全部关卡完成。{0}" },
            { "level.state.locked", "锁定" }, { "level.state.completed", "完成" }, { "level.state.continue", "继续" }, { "level.state.current", "进行中" },
            { "completion.clear.yes", "已通关" }, { "completion.clear.no", "未通关" }, { "completion.coins.none", "无金币" }, { "completion.coins.yes", "全部金币" }, { "completion.coins.no", "金币未齐" }, { "completion.clean.yes", "无伤路线" }, { "completion.clean.no", "无伤未达成" },
            { "clean.reason.damage", "受伤" }, { "clean.reason.restart", "重开" }, { "level.feature.14", "升降台" }, { "level.feature.15", "碎裂" },
            { "level.name.1", "关卡 1：滚动课程" }, { "level.name.2", "关卡 2：第一处危险" }, { "level.name.3", "关卡 3：弹跳课程" }, { "level.name.4", "关卡 4：窄步道" }, { "level.name.5", "关卡 5：巡逻路线" }, { "level.name.6", "关卡 6：移动桥" }, { "level.name.7", "关卡 7：弹跳阶梯" }, { "level.name.8", "关卡 8：节奏桥" }, { "level.name.9", "关卡 9：高线" }, { "level.name.10", "关卡 10：下坡出口" }, { "level.name.11", "关卡 11：巡逻门" }, { "level.name.12", "关卡 12：精准弹跳" }, { "level.name.13", "关卡 13：最终奔跑" }, { "level.name.14", "关卡 14：升降花园" }, { "level.name.15", "关卡 15：碎石" }
        });
        AddLanguage("hi", en, new Dictionary<string, string> {
            { "menu.banner", "SPRINT 03 आधुनिक UI" }, { "menu.title", "रेड बॉल\nमास्टरी" }, { "menu.subtitle", "15 हाथ से बने लेवल और बैज" },
            { "menu.feature.lifts", "लिफ्ट टाइमिंग" }, { "menu.feature.crumble", "टूटने वाली टाइलें" }, { "menu.feature.badges", "क्लियर / कॉइन / क्लीन" },
            { "button.continue", "जारी रखें" }, { "button.levels", "लेवल" }, { "button.back", "वापस" }, { "button.language", "भाषा: {0}" },
            { "levelSelect.title", "लेवल चुनें" }, { "levelSelect.subtitle", "रूट चुनें, बैज लें, और अंतिम mechanics देखें." }, { "levelSelect.legend", "बैज: क्लियर, सभी कॉइन, क्लीन रन. खास कार्ड लिफ्ट और टूटने वाले रूट दिखाते हैं." },
            { "level.tag.lift", "लिफ्ट" }, { "level.tag.crumble", "टूटता" }, { "hud.health", "स्वास्थ्य" }, { "hud.score", "स्कोर" }, { "hud.coin", "कॉइन" }, { "hud.level", "लेवल" },
            { "life.full", "पूरा" }, { "life.next", "अगला" }, { "time.hourShort", "1घं" }, { "time.minShort", "{0}मि" },
            { "status.noHearts", "दिल नहीं हैं. अगला: {0}" }, { "status.locked", "यह लेवल लॉक है. पहले पिछला लेवल पूरा करें." }, { "status.heartsExhausted", "दिल खत्म. हर घंटे एक दिल वापस आता है." },
            { "message.noHearts", "कोई दिल नहीं बचा. 1 घंटे में एक दिल आएगा." }, { "message.damage", "सावधान! एक दिल गया और आप checkpoint पर लौटे." }, { "message.checkpoint", "Checkpoint!" }, { "message.badgesPrefix", "बैज:" },
            { "message.levelComplete", "लेवल पूरा! {0}" }, { "message.gameComplete", "बधाई! सभी लेवल पूरे. {0}" },
            { "level.state.locked", "लॉक" }, { "level.state.completed", "पूरा" }, { "level.state.continue", "जारी" }, { "level.state.current", "खेल रहे" },
            { "completion.clear.yes", "क्लियर मिला" }, { "completion.clear.no", "क्लियर बाकी" }, { "completion.coins.none", "कॉइन नहीं" }, { "completion.coins.yes", "सभी कॉइन" }, { "completion.coins.no", "कॉइन बाकी" }, { "completion.clean.yes", "क्लीन रन" }, { "completion.clean.no", "क्लीन बाकी" },
            { "clean.reason.damage", "डैमेज" }, { "clean.reason.restart", "रीस्टार्ट" }, { "level.feature.14", "लिफ्ट" }, { "level.feature.15", "टूटता" },
            { "level.name.1", "लेवल 1: रोलिंग पाठ" }, { "level.name.2", "लेवल 2: पहला खतरा" }, { "level.name.3", "लेवल 3: बाउंस पाठ" }, { "level.name.4", "लेवल 4: पतले कदम" }, { "level.name.5", "लेवल 5: पेट्रोल रूट" }, { "level.name.6", "लेवल 6: चलती पुलिया" }, { "level.name.7", "लेवल 7: बाउंस सीढ़ी" }, { "level.name.8", "लेवल 8: रिदम ब्रिज" }, { "level.name.9", "लेवल 9: हाई लाइन" }, { "level.name.10", "लेवल 10: उतराई निकास" }, { "level.name.11", "लेवल 11: पेट्रोल गेट" }, { "level.name.12", "लेवल 12: तेज बाउंस" }, { "level.name.13", "लेवल 13: अंतिम दौड़" }, { "level.name.14", "लेवल 14: लिफ्ट गार्डन" }, { "level.name.15", "लेवल 15: टूटे पत्थर" }
        });
        AddLanguage("ja", en, new Dictionary<string, string> {
            { "menu.banner", "SPRINT 03 モダンUI" }, { "menu.title", "レッドボール\nマスタリー" }, { "menu.subtitle", "15ステージとマスタリーバッジ" },
            { "menu.feature.lifts", "リフトのタイミング" }, { "menu.feature.crumble", "崩れるタイル" }, { "menu.feature.badges", "クリア / コイン / クリーン" },
            { "button.continue", "続ける" }, { "button.levels", "レベル" }, { "button.back", "戻る" }, { "button.language", "言語: {0}" },
            { "levelSelect.title", "レベル選択" }, { "levelSelect.subtitle", "ルートを選び、バッジを狙い、終盤ギミックを確認。" }, { "levelSelect.legend", "バッジ: クリア、全コイン、クリーンラン。注目カードはリフトと崩れるルートを表示。" },
            { "level.tag.lift", "リフト" }, { "level.tag.crumble", "崩れる" }, { "hud.health", "体力" }, { "hud.score", "スコア" }, { "hud.coin", "コイン" }, { "hud.level", "レベル" },
            { "life.full", "満タン" }, { "life.next", "次" }, { "time.hourShort", "1時間" }, { "time.minShort", "{0}分" },
            { "status.noHearts", "ハートなし。次: {0}" }, { "status.locked", "このレベルはロック中です。前のレベルをクリアしてください。" }, { "status.heartsExhausted", "ハート切れ。1時間ごとに1つ戻ります。" },
            { "message.noHearts", "ハートがありません。1時間後に1つ戻ります。" }, { "message.damage", "注意！ハートを失い checkpoint に戻りました。" }, { "message.checkpoint", "Checkpoint!" }, { "message.badgesPrefix", "バッジ:" },
            { "message.levelComplete", "レベルクリア！{0}" }, { "message.gameComplete", "おめでとう！全レベルクリア。{0}" },
            { "level.state.locked", "ロック" }, { "level.state.completed", "クリア" }, { "level.state.continue", "続き" }, { "level.state.current", "プレイ中" },
            { "completion.clear.yes", "クリア取得" }, { "completion.clear.no", "クリア未取得" }, { "completion.coins.none", "コインなし" }, { "completion.coins.yes", "全コイン" }, { "completion.coins.no", "コイン不足" }, { "completion.clean.yes", "クリーンラン" }, { "completion.clean.no", "クリーン未達成" },
            { "clean.reason.damage", "ダメージ" }, { "clean.reason.restart", "再スタート" }, { "level.feature.14", "リフト" }, { "level.feature.15", "崩れる" },
            { "level.name.1", "レベル1：転がりレッスン" }, { "level.name.2", "レベル2：最初の危険" }, { "level.name.3", "レベル3：バウンス練習" }, { "level.name.4", "レベル4：細い足場" }, { "level.name.5", "レベル5：巡回ルート" }, { "level.name.6", "レベル6：動く橋" }, { "level.name.7", "レベル7：バウンス階段" }, { "level.name.8", "レベル8：リズム橋" }, { "level.name.9", "レベル9：高いライン" }, { "level.name.10", "レベル10：下り出口" }, { "level.name.11", "レベル11：巡回ゲート" }, { "level.name.12", "レベル12：鋭いバウンス" }, { "level.name.13", "レベル13：最終ラン" }, { "level.name.14", "レベル14：リフト庭園" }, { "level.name.15", "レベル15：壊れた石" }
        });
    }
}
