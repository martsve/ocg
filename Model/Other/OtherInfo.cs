using System;

namespace Delver
{

    enum PopulateResult
    {
        Accepted,
        NoneSelected,
        NoLegalTargets,
    }


    public enum InteractionType
    {
        Pass,
        Accept,
        Cast,
        Ability,
        GetView,
        Replay
    }

    public enum Keywords
    {
        FirstStrike,
        DoubleStrike,
        Haste,
        Prowess,
        Indestructible,
        Trample,
        Flying,
        Flash,
        Phasing,
        Deathtouch,
        Defender,
        Hexproof,
        Lifelink,
        Menace,
        Reach,
        Scry,
        Vigilance,
        Absorb,
        Affinity,
        Amplify,
        Annihilator,
        Bands,
        BattleCry,
        Bestow,
        Bolster,
        Bloodthirst,
        Bushido,
        Buyback,
        Cascade,
        Champion,
        Changeling,
        Cipher,
        Clash,
        Conspire,
        Convoke,
        Cycling,
        Dash,
        Delve,
        Detain,
        Devour,
        Dredge,
        Echo,
        Entwine,
        Epic,
        Evolve,
        Evoke,
        Exalted,
        Exploit,
        Extort,
        Fading,
        Fateseal,
        Flanking,
        Flashback,
        Forecast,
        Fortify,
        Frenzy,
        Graft,
        Gravestorm,
        Haunt,
        Hideaway,
        Horsemanship,
        Infect,
        Kicker,
        Landfall,
        LevelUp,
        LivingWeapon,
        Madness,
        Manifest,
        Miracle,
        Modular,
        Monstrosity,
        Morph,
        Multikicker,
        Ninjutsu,
        Offering,
        Overload,
        Persist,
        Poisonous,
        Populate,
        Proliferate,
        Provoke,
        Prowl,
        Rampage,
        Rebound,
        Recover,
        Reinforce,
        Renown,
        Replicate,
        Retrace,
        Ripple,
        Scavenge,
        Shadow,
        Soulbond,
        Soulshift,
        Splice,
        SplitSecond,
        Storm,
        Sunburst,
        Suspend,
        TotemArmor,
        Transfigure,
        Transform,
        Transmute,
        Typecycling,
        Undying,
        Unearth,
        Unleash,
        Vanishing,
        Wither,
        Battalion,
        Bloodrush,
        Channel,
        Chroma,
        Domain,
        FatefulHour,
        Ferocious,
        Grandeur,
        Hellbent,
        Heroic,
        Imprint,
        JoinForces,
        Kinship,
        Metalcraft,
        Morbid,
        Radiance,
        Raid,
        Sweep,
        Threshold,
        Banding,
        Fear,
        Intimidate,
        Landhome,
        Landwalk,
        Protection,
        Shroud,
        Substance
    }


    public enum Duration
    {
        EndOfTurn,
        NextCleanup,
        NextEndStep,
        Continous,
        Following,
        Emblem
    }

    public enum Zone
    {
        Stack,
        Battlefield,
        Hand,
        Exile,
        Library,
        Graveyard,
        Command,
        None,
        Global
    }

    [Flags]
    public enum CardType
    {
        None = 0,
        Land = 1,
        Creature = 2,
        Instant = 4,
        Sorcery = 8,
        Planeswalker = 16,
        Enchantment = 32,
        Artifact = 64,
        Tribal = 128,
        Permanent = 256,
        Basic = 512,
        Legendary = 1024,
        Ability = 2048,
        Token = 4096,
        Aura = 8192,

        Any = 8192 * 2 - 1
    }

    [Flags]
    public enum Identity
    {
        None = 0,
        White = 1,
        Blue = 2,
        Black = 4,
        Red = 8,
        Green = 16,
        Colorless = 32,
        Generic = 64
    }
}