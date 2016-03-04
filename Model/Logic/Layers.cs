﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delver
{
    enum Layers
    {
        // 613.1a Layer 1: Copy effects are applied. See rule 706, “Copying Objects.”
        CopyEffects = 1,
        // 613.1b Layer 2: Control-changing effects are applied.
        ControlChanging = 2,
        //613.1c Layer 3: Text-changing effects are applied. See rule 612, “Text-Changing Effects.”
        TextChanging = 3,
        //613.1d Layer 4: Type-changing effects are applied. These include effects that change an object’s card type, subtype, and/or supertype.
        TypeChanging = 4,
        //613.1e Layer 5: Color-changing effects are applied.
        ColorChanging = 5,
        //613.1f Layer 6: Ability-adding effects, ability-removing effects, and effects that say an object can’t have an ability are applied.
        AbilityAdding = 6,

        //613.1g Layer 7: Power- and/or toughness-changing effects are applied.

        //613.3a Layer 7a: Effects from characteristic-defining abilities that define power and/or toughness are applied. See rule 604.3.
        PowerChanging_A_Define = 7,
        //613.3b Layer 7b: Effects that set power and/or toughness to a specific number or value are applied. Effects that refer to the base power and/or toughness of a creature apply in this layer.
        PowerChanging_B_Set = 8,
        //613.3c Layer 7c: Effects that modify power and/or toughness (but don’t set power and/or toughness to a specific number or value) are applied.
        PowerChanging_C_Modify = 9,
        //613.3d Layer 7d: Power and/or toughness changes from counters are applied. See rule 121, “Counters.”
        PowerChanging_D_Counters = 10,
        //613.3e Layer 7e: Effects that switch a creature’s power and toughness are applied. Such effects take the value of power and apply it to the creature’s toughness, and take the value of toughness and apply it to the creature’s power.
        PowerChanging_E_Switch = 11,
    }

    [Serializable]
    class LayeredEffect
    {
        public Duration Duration { get; set; }

        //613.7. Within a layer or sublayer, determining which order effects are applied in is sometimes done using a dependency system. If a dependency exists, it will override the timestamp system.
        //613.7a An effect is said to “depend on” another if (a) it’s applied in the same layer(and, if applicable, sublayer) as the other effect(see rules 613.1 and 613.3); (b) applying the other would change the text or the existence of the first effect, what it applies to, or what it does to any of the things it applies to; and(c) neither effect is from a characteristic-defining ability or both effects are from characteristic-defining abilities.Otherwise, the effect is considered to be independent of the other effect.
        //613.7b An effect dependent on one or more other effects waits to apply until just after all of those effects have been applied.If multiple dependent effects would apply simultaneously in this way, they’re applied in timestamp order relative to each other. If several dependent effects form a dependency loop, then this rule is ignored and the effects in the dependency loop are applied in timestamp order.
        //613.7c After each effect is applied, the order of remaining effects is reevaluated and may change if an effect that has not yet been applied becomes dependent on or independent of one or more other effects that have not yet been applied.
        public Layers Layer { get; set; }

        //613.6. Within a layer or sublayer, determining which order effects are applied in is usually done using a timestamp system. An effect with an earlier timestamp is applied before an effect with a later timestamp.
        //613.6a A continuous effect generated by a static ability has the same timestamp as the object the static ability is on, or the timestamp of the effect that created the ability, whichever is later.
        //613.6b A continuous effect generated by the resolution of a spell or ability receives a timestamp at the time it’s created.
        //613.6c An object receives a timestamp at the time it enters a zone.
        //613.6d An Aura, Equipment, or Fortification receives a new timestamp at the time it becomes attached to an object or player.
        //613.6e A permanent receives a new timestamp at the time it turns face up or face down.
        //613.6f A double-faced permanent receives a new timestamp at the time it transforms.
        //613.6g A face-up plane card, phenomenon card, or scheme card receives a timestamp at the time it’s turned face up.
        //613.6h A face-up vanguard card receives a timestamp at the beginning of the game.
        //613.6i A conspiracy card receives a timestamp at the beginning of the game.If it’s face down, it receives a new timestamp at the time it turns face up.
        //613.6j If two or more objects would receive a timestamp simultaneously, such as by entering a zone simultaneously or becoming attached simultaneously, the active player determines their relative timestamp order at that time.
        public int Timestamp { get; set; }

        public ReferanceList<Card> AffectedCards { get; set; }
        public ReferanceList<GameObject> AffectedObjects { get; set; }
        public ReferanceList<Player> AffectedPlayers { get; set; }

        public LayeredEffect(Duration duration, Layers layer)
        {
            this.Layer = layer;
            this.Duration = duration;
        }

        public virtual void Invoke()
        {
        }

        public virtual void End()
        {
        }
    }


}
