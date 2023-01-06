using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace RiotStatsService_FunctionApp
{
    public partial class MatchDataModel
    {
        [JsonProperty("metadata")]
        public Metadata Metadata { get; set; }

        [JsonProperty("info")]
        public Info Info { get; set; }
    }

    public partial class Info
    {
        [JsonProperty("gameCreation")]
        public long GameCreation { get; set; }

        [JsonProperty("gameDuration")]
        public long GameDuration { get; set; }

        [JsonProperty("gameEndTimestamp")]
        public long GameEndTimestamp { get; set; }

        [JsonProperty("gameId")]
        public long GameId { get; set; }

        [JsonProperty("gameMode")]
        public string GameMode { get; set; }

        [JsonProperty("gameName")]
        public string GameName { get; set; }

        [JsonProperty("gameStartTimestamp")]
        public long GameStartTimestamp { get; set; }

        [JsonProperty("gameType")]
        public string GameType { get; set; }

        [JsonProperty("gameVersion")]
        public string GameVersion { get; set; }

        [JsonProperty("mapId")]
        public long MapId { get; set; }

        [JsonProperty("participants")]
        public List<Participant> Participants { get; set; }

        [JsonProperty("platformId")]
        public string PlatformId { get; set; }

        [JsonProperty("queueId")]
        public long QueueId { get; set; }

        [JsonProperty("teams")]
        public List<Team> Teams { get; set; }

        [JsonProperty("tournamentCode")]
        public string TournamentCode { get; set; }
    }

    public partial class Participant
    {
        [JsonProperty("assists")]
        public int Assists { get; set; }

        [JsonProperty("baronKills")]
        public long BaronKills { get; set; }

        [JsonProperty("bountyLevel")]
        public long BountyLevel { get; set; }

        [JsonProperty("champExperience")]
        public long ChampExperience { get; set; }

        [JsonProperty("champLevel")]
        public long ChampLevel { get; set; }

        [JsonProperty("championId")]
        public long ChampionId { get; set; }

        [JsonProperty("championName")]
        public string ChampionName { get; set; }

        [JsonProperty("championTransform")]
        public long ChampionTransform { get; set; }

        [JsonProperty("consumablesPurchased")]
        public long ConsumablesPurchased { get; set; }

        [JsonProperty("damageDealtToBuildings")]
        public long DamageDealtToBuildings { get; set; }

        [JsonProperty("damageDealtToObjectives")]
        public long DamageDealtToObjectives { get; set; }

        [JsonProperty("damageDealtToTurrets")]
        public long DamageDealtToTurrets { get; set; }

        [JsonProperty("damageSelfMitigated")]
        public long DamageSelfMitigated { get; set; }

        [JsonProperty("deaths")]
        public int Deaths { get; set; }

        [JsonProperty("detectorWardsPlaced")]
        public long DetectorWardsPlaced { get; set; }

        [JsonProperty("doubleKills")]
        public long DoubleKills { get; set; }

        [JsonProperty("dragonKills")]
        public long DragonKills { get; set; }

        [JsonProperty("firstBloodAssist")]
        public bool FirstBloodAssist { get; set; }

        [JsonProperty("firstBloodKill")]
        public bool FirstBloodKill { get; set; }

        [JsonProperty("firstTowerAssist")]
        public bool FirstTowerAssist { get; set; }

        [JsonProperty("firstTowerKill")]
        public bool FirstTowerKill { get; set; }

        [JsonProperty("gameEndedInEarlySurrender")]
        public bool GameEndedInEarlySurrender { get; set; }

        [JsonProperty("gameEndedInSurrender")]
        public bool GameEndedInSurrender { get; set; }

        [JsonProperty("goldEarned")]
        public int GoldEarned { get; set; }

        [JsonProperty("goldSpent")]
        public int GoldSpent { get; set; }

        [JsonProperty("individualPosition")]
        public IndividualPosition IndividualPosition { get; set; }

        [JsonProperty("inhibitorKills")]
        public long InhibitorKills { get; set; }

        [JsonProperty("inhibitorTakedowns")]
        public long InhibitorTakedowns { get; set; }

        [JsonProperty("inhibitorsLost")]
        public long InhibitorsLost { get; set; }

        [JsonProperty("item0")]
        public long Item0 { get; set; }

        [JsonProperty("item1")]
        public long Item1 { get; set; }

        [JsonProperty("item2")]
        public long Item2 { get; set; }

        [JsonProperty("item3")]
        public long Item3 { get; set; }

        [JsonProperty("item4")]
        public long Item4 { get; set; }

        [JsonProperty("item5")]
        public long Item5 { get; set; }

        [JsonProperty("item6")]
        public long Item6 { get; set; }

        [JsonProperty("itemsPurchased")]
        public long ItemsPurchased { get; set; }

        [JsonProperty("killingSprees")]
        public long KillingSprees { get; set; }

        [JsonProperty("kills")]
        public int Kills { get; set; }

        [JsonProperty("lane")]
        public String Lane { get; set; }

        [JsonProperty("largestCriticalStrike")]
        public long LargestCriticalStrike { get; set; }

        [JsonProperty("largestKillingSpree")]
        public long LargestKillingSpree { get; set; }

        [JsonProperty("largestMultiKill")]
        public long LargestMultiKill { get; set; }

        [JsonProperty("longestTimeSpentLiving")]
        public long LongestTimeSpentLiving { get; set; }

        [JsonProperty("magicDamageDealt")]
        public long MagicDamageDealt { get; set; }

        [JsonProperty("magicDamageDealtToChampions")]
        public long MagicDamageDealtToChampions { get; set; }

        [JsonProperty("magicDamageTaken")]
        public long MagicDamageTaken { get; set; }

        [JsonProperty("neutralMinionsKilled")]
        public long NeutralMinionsKilled { get; set; }

        [JsonProperty("nexusKills")]
        public long NexusKills { get; set; }

        [JsonProperty("nexusLost")]
        public long NexusLost { get; set; }

        [JsonProperty("nexusTakedowns")]
        public long NexusTakedowns { get; set; }

        [JsonProperty("objectivesStolen")]
        public long ObjectivesStolen { get; set; }

        [JsonProperty("objectivesStolenAssists")]
        public long ObjectivesStolenAssists { get; set; }

        [JsonProperty("participantId")]
        public long ParticipantId { get; set; }

        [JsonProperty("pentaKills")]
        public long PentaKills { get; set; }

        [JsonProperty("perks")]
        public Perks Perks { get; set; }

        [JsonProperty("physicalDamageDealt")]
        public long PhysicalDamageDealt { get; set; }

        [JsonProperty("physicalDamageDealtToChampions")]
        public long PhysicalDamageDealtToChampions { get; set; }

        [JsonProperty("physicalDamageTaken")]
        public long PhysicalDamageTaken { get; set; }

        [JsonProperty("profileIcon")]
        public long ProfileIcon { get; set; }

        [JsonProperty("puuid")]
        public string Puuid { get; set; }

        [JsonProperty("quadraKills")]
        public long QuadraKills { get; set; }

        [JsonProperty("riotIdName")]
        public string RiotIdName { get; set; }

        [JsonProperty("riotIdTagline")]
        public string RiotIdTagline { get; set; }

        [JsonProperty("role")]
        public String Role { get; set; }

        [JsonProperty("sightWardsBoughtInGame")]
        public long SightWardsBoughtInGame { get; set; }

        [JsonProperty("spell1Casts")]
        public long Spell1Casts { get; set; }

        [JsonProperty("spell2Casts")]
        public long Spell2Casts { get; set; }

        [JsonProperty("spell3Casts")]
        public long Spell3Casts { get; set; }

        [JsonProperty("spell4Casts")]
        public long Spell4Casts { get; set; }

        [JsonProperty("summoner1Casts")]
        public long Summoner1Casts { get; set; }

        [JsonProperty("summoner1Id")]
        public long Summoner1Id { get; set; }

        [JsonProperty("summoner2Casts")]
        public long Summoner2Casts { get; set; }

        [JsonProperty("summoner2Id")]
        public long Summoner2Id { get; set; }

        [JsonProperty("summonerId")]
        public string SummonerId { get; set; }

        [JsonProperty("summonerLevel")]
        public long SummonerLevel { get; set; }

        [JsonProperty("summonerName")]
        public string SummonerName { get; set; }

        [JsonProperty("teamEarlySurrendered")]
        public bool TeamEarlySurrendered { get; set; }

        [JsonProperty("teamId")]
        public long TeamId { get; set; }

        [JsonProperty("teamPosition")]
        public string TeamPosition { get; set; }

        [JsonProperty("timeCCingOthers")]
        public long TimeCCingOthers { get; set; }

        [JsonProperty("timePlayed")]
        public long TimePlayed { get; set; }

        [JsonProperty("totalDamageDealt")]
        public long TotalDamageDealt { get; set; }

        [JsonProperty("totalDamageDealtToChampions")]
        public long TotalDamageDealtToChampions { get; set; }

        [JsonProperty("totalDamageShieldedOnTeammates")]
        public long TotalDamageShieldedOnTeammates { get; set; }

        [JsonProperty("totalDamageTaken")]
        public long TotalDamageTaken { get; set; }

        [JsonProperty("totalHeal")]
        public long TotalHeal { get; set; }

        [JsonProperty("totalHealsOnTeammates")]
        public int TotalHealsOnTeammates { get; set; }

        [JsonProperty("totalMinionsKilled")]
        public long TotalMinionsKilled { get; set; }

        [JsonProperty("totalTimeCCDealt")]
        public long TotalTimeCcDealt { get; set; }

        [JsonProperty("totalTimeSpentDead")]
        public long TotalTimeSpentDead { get; set; }

        [JsonProperty("totalUnitsHealed")]
        public long TotalUnitsHealed { get; set; }

        [JsonProperty("tripleKills")]
        public long TripleKills { get; set; }

        [JsonProperty("trueDamageDealt")]
        public long TrueDamageDealt { get; set; }

        [JsonProperty("trueDamageDealtToChampions")]
        public long TrueDamageDealtToChampions { get; set; }

        [JsonProperty("trueDamageTaken")]
        public long TrueDamageTaken { get; set; }

        [JsonProperty("turretKills")]
        public long TurretKills { get; set; }

        [JsonProperty("turretTakedowns")]
        public long TurretTakedowns { get; set; }

        [JsonProperty("turretsLost")]
        public long TurretsLost { get; set; }

        [JsonProperty("unrealKills")]
        public long UnrealKills { get; set; }

        [JsonProperty("visionScore")]
        public long VisionScore { get; set; }

        [JsonProperty("visionWardsBoughtInGame")]
        public long VisionWardsBoughtInGame { get; set; }

        [JsonProperty("wardsKilled")]
        public long WardsKilled { get; set; }

        [JsonProperty("wardsPlaced")]
        public long WardsPlaced { get; set; }

        [JsonProperty("win")]
        public bool Win { get; set; }
    }

    public partial class Perks
    {
        [JsonProperty("statPerks")]
        public StatPerks StatPerks { get; set; }

        [JsonProperty("styles")]
        public List<Style> Styles { get; set; }
    }

    public partial class StatPerks
    {
        [JsonProperty("defense")]
        public long Defense { get; set; }

        [JsonProperty("flex")]
        public long Flex { get; set; }

        [JsonProperty("offense")]
        public long Offense { get; set; }
    }

    public partial class Style
    {
        [JsonProperty("description")]
        public Description Description { get; set; }

        [JsonProperty("selections")]
        public List<Selection> Selections { get; set; }

        [JsonProperty("style")]
        public long StyleStyle { get; set; }
    }

    public partial class Selection
    {
        [JsonProperty("perk")]
        public long Perk { get; set; }

        [JsonProperty("var1")]
        public long Var1 { get; set; }

        [JsonProperty("var2")]
        public long Var2 { get; set; }

        [JsonProperty("var3")]
        public long Var3 { get; set; }
    }

    public partial class Team
    {
        [JsonProperty("bans")]
        public List<object> Bans { get; set; }

        [JsonProperty("objectives")]
        public Objectives Objectives { get; set; }

        [JsonProperty("teamId")]
        public long TeamId { get; set; }

        [JsonProperty("win")]
        public bool Win { get; set; }
    }

    public partial class Objectives
    {
        [JsonProperty("baron")]
        public Baron Baron { get; set; }

        [JsonProperty("champion")]
        public Baron Champion { get; set; }

        [JsonProperty("dragon")]
        public Baron Dragon { get; set; }

        [JsonProperty("inhibitor")]
        public Baron Inhibitor { get; set; }

        [JsonProperty("riftHerald")]
        public Baron RiftHerald { get; set; }

        [JsonProperty("tower")]
        public Baron Tower { get; set; }
    }

    public partial class Baron
    {
        [JsonProperty("first")]
        public bool First { get; set; }

        [JsonProperty("kills")]
        public long Kills { get; set; }
    }

    public partial class Metadata
    {
        [JsonProperty("dataVersion")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long DataVersion { get; set; }

        [JsonProperty("matchId")]
        public string MatchId { get; set; }

        [JsonProperty("participants")]
        public List<string> Participants { get; set; }
    }

    public enum IndividualPosition { Invalid };

    public enum Lane { None };

    public enum Description { PrimaryStyle, SubStyle };

    public enum Role { Duo, Support };

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                IndividualPositionConverter.Singleton,
                LaneConverter.Singleton,
                DescriptionConverter.Singleton,
                RoleConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class IndividualPositionConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(IndividualPosition) || t == typeof(IndividualPosition?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (value == "Invalid")
            {
                return IndividualPosition.Invalid;
            }
            throw new Exception("Cannot unmarshal type IndividualPosition");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (IndividualPosition)untypedValue;
            if (value == IndividualPosition.Invalid)
            {
                serializer.Serialize(writer, "Invalid");
                return;
            }
            throw new Exception("Cannot marshal type IndividualPosition");
        }

        public static readonly IndividualPositionConverter Singleton = new IndividualPositionConverter();
    }

    internal class LaneConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Lane) || t == typeof(Lane?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (value == "NONE")
            {
                return Lane.None;
            }
            throw new Exception("Cannot unmarshal type Lane");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Lane)untypedValue;
            if (value == Lane.None)
            {
                serializer.Serialize(writer, "NONE");
                return;
            }
            throw new Exception("Cannot marshal type Lane");
        }

        public static readonly LaneConverter Singleton = new LaneConverter();
    }

    internal class DescriptionConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Description) || t == typeof(Description?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "primaryStyle":
                    return Description.PrimaryStyle;
                case "subStyle":
                    return Description.SubStyle;
            }
            throw new Exception("Cannot unmarshal type Description");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Description)untypedValue;
            switch (value)
            {
                case Description.PrimaryStyle:
                    serializer.Serialize(writer, "primaryStyle");
                    return;
                case Description.SubStyle:
                    serializer.Serialize(writer, "subStyle");
                    return;
            }
            throw new Exception("Cannot marshal type Description");
        }

        public static readonly DescriptionConverter Singleton = new DescriptionConverter();
    }

    internal class RoleConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Role) || t == typeof(Role?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "DUO":
                    return Role.Duo;
                case "SUPPORT":
                    return Role.Support;
            }
            throw new Exception("Cannot unmarshal type Role");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Role)untypedValue;
            switch (value)
            {
                case Role.Duo:
                    serializer.Serialize(writer, "DUO");
                    return;
                case Role.Support:
                    serializer.Serialize(writer, "SUPPORT");
                    return;
            }
            throw new Exception("Cannot marshal type Role");
        }

        public static readonly RoleConverter Singleton = new RoleConverter();
    }

    internal class ParseStringConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(long) || t == typeof(long?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            long l;
            if (Int64.TryParse(value, out l))
            {
                return l;
            }
            throw new Exception("Cannot unmarshal type long");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (long)untypedValue;
            serializer.Serialize(writer, value.ToString());
            return;
        }

        public static readonly ParseStringConverter Singleton = new ParseStringConverter();
    }
}
