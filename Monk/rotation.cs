using System.Linq;
using Shaper;
using System.Diagnostics;

namespace Shaper
{
    public class HWalker : Rotation
        
    {
        public override void Initialize()
        {
            API.KeypressDelay = 333;
            API.PrintLine(" ------------------------------------------------------------");
            API.PrintLine(" ------------ Welcome to HMonk v1.0.0 by Hivro --------------");
            API.PrintLine(" ----- Please let me know if you have any suggestions -------");
            API.PrintLine(" --- and I will work on them as best as I can, thank you! ---");
            API.PrintLine(" ------------------------------------------------------------");
            API.PrintLine(" ------------ WW = 30% --- BrM = 20% --- MW = N/A -----------");
            PulseTimer.Start();
        }

        public static Stopwatch PulseTimer = new Stopwatch();

        //immune buffs for focus dps, these also must be listed in TrackBuffs.cfg -----------------
        public static string[] immunes =
        {
            "Divine Shield", "Nether Ward", "Anti-Magic Shell", "Spell Reflection", "Ice Block", "Cloak of Shadows", "Aspect of the Turtle", "Netherwalk", "Touch of Karma"
        };

        // cc debuffs we don't want to break for focus dps, these also must be listed in TrackDebuffs.cfg
        public static string[] ccs =
        {
            "Blind", "Polymorph", "Fear", "Psychic Scream", "Sap", "Hex", "Freezing Trap", "Cyclone", "Paralysis"
        };

        //set a GCD timer
        public static int MaxGCD = 0;
        
        //Create a spell priority casting list and past list of casted spells
        private static int Priority;
        public override void CombatPulse()

        {

            //initialize some commonly used variables at the beginning of each pulse to cut down on API calls
            bool IgnoreTarget = false;
            bool Moving = API.IsMoving();
            int TargetHealth = API.Health("target");

            //Rescources --------------------------------------------------------------------------
            int Energy = API.Power();
            int Chi = API.SecondaryPower();

            //GCD
            int CurrentGCD = API.GCD();
            if (CurrentGCD > MaxGCD)
                MaxGCD = CurrentGCD;

            //Do or Do not Hit the target ---------------------------------------------------------
            for (int i = 0; i < immunes.Length; i++) // ignore our focus dps stuff if focus is cc'd or immune
            {
                if (API.HasBuff(immunes[i], "target", false))
                    IgnoreTarget = true;
            }

            for (int i = 0; i < ccs.Length; i++)
            {
                if (API.HasDebuff(ccs[i], "target", false))
                    IgnoreTarget = true;
            }

            //If target shouldnt be hit -- display message to tab if needed -----------------------


            //=====================================================================================
            //Windwalker
            //=====================================================================================
            if (API.Spec() == "Monk: Windwalker")
            {
                //WW TALENTS ----------------------------------------------------------------------
                bool ChiBurst = (API.Talents(1) == 1);
                bool EyeoftheTiger = (API.Talents(1) == 2);
                bool ChiWave = (API.Talents(1) == 3);
                bool ChiTorpedo = (API.Talents(2) == 1);
                bool TigersLust = (API.Talents(2) == 2);
                bool Celerity = (API.Talents(2) == 3);
                bool EnergizingElixir = (API.Talents(3) == 1);
                bool Ascension = (API.Talents(3) == 2);
                bool PowerStrikes = (API.Talents(3) == 3);
                bool RingofPeace = (API.Talents(4) == 1);
                bool SummonBlackOxStatue = (API.Talents(4) == 2);
                bool LegSweep = (API.Talents(4) == 3);
                bool HealingElixir = (API.Talents(5) == 1);
                bool DiffuseMagic = (API.Talents(5) == 2);
                bool DampenHarm = (API.Talents(5) == 3);
                bool RushingJadeWind = (API.Talents(6) == 1);
                bool InvokeXuen = (API.Talents(6) == 2);
                bool HitCombo = (API.Talents(6) == 3);
                bool ChiOrbit = (API.Talents(7) == 1);
                bool WhirlingDragonPunch = (API.Talents(7) == 2);
                bool Serenity = (API.Talents(7) == 3);

                //WW Macros -----------------------------------------------------------------------
                //API.CreateMacro("Self Effuse", "/cast [@player] Effuse");

                

                // WW Rotation Start --------------------------------------------------------------
                if ((API.CanCast("Tiger Palm") && API.TargetIsEnemy() && (!IgnoreTarget)) && (API.Spec() == "Monk: Windwalker"))
                {
                    //Serenity Burst Logic --------------------------------------------------------
                    /* if (API.HasBuff("Serenity"))
                        {
                            
                        }
                        */
                    //Basic Rotation --------------------------------------------------------------
                    
                    //Fists of Fury
                    if (API.CanCast("Fists of Fury", "target") && (Chi >= 3) && (API.LastCastSuccess() != "Energizing Elixir"))
                    {
                        API.Cast("Fists of Fury");
                        return;
                    }
                    
                    //Strike Of The Windlord
                    if (API.CanCast("Strike of the Windlord", "target") && Chi >= 2)
                    {
                        API.Cast("Strike of the Windlord");                            
                        return;
                    }
                    
                    //Tiger Palm (if <4 Chi and about to cap energy)
                    if (API.CanCast("Tiger Palm", "target") && Energy >= 50 && (API.LastCastSuccess() != ("Energizing Elixir")) && Chi < 4 && (API.LastCastSuccess() != "Tiger Palm"))
                    {
                        API.Cast("Tiger Palm");
                        return;
                    }

                    //Rising Sun Kick
                    if (API.CanCast("Rising Sun Kick", "target") && Chi >= 2)
                    {
                        API.Cast("Rising Sun Kick");
                        return;
                    }

                    //Whirling Dragon Punch
                    if (API.CanCast("Whirling Dragon Punch", "target") && (API.SpellCooldown("Fists of Fury") < MaxGCD * .28) && (API.SpellCooldown("Rising Sun Kick") < MaxGCD * .28))
                    {
                        API.Cast("Whirling Dragon Punch");
                        return;
                    }

                    //Chi Wave
                    if (API.CanCast("Chi Wave", "target") && Energy < 50 && Chi < 5)
                    {
                        API.Cast("Chi Wave");
                        return;
                    }

                    //Blackout Kick
                    if (API.CanCast("Blackout Kick", "target") && (Chi > 1 || API.HasBuff("Blackout Kick!")) && (API.LastCastSuccess() != "Blackout Kick"))
                    {
                        API.Cast("Blackout Kick");
                        return;
                    }

                    //Tiger Palm
                    if (API.CanCast("Tiger Palm", "target") && Energy >= 50 && (API.LastCastSuccess() != "Tiger Palm"))
                    {
                        API.Cast("Tiger Palm");
                        return;
                    }

                    //CracklingJadeLightning (only to counter the Tiger Palm issue)
                    if (API.CanCast("Crackling Jade Lightning", "target") && Energy >= 20 && (API.LastCastSuccess() != "Tiger Palm") && Chi == 0)
                    {
                        API.Cast("Crackling Jade Lightning");
                        return;
                    }
                        
                    //WW AoE Rotation -------------------------------------------------------------
                    //if ((EnemiesInMeleeRange() > 1) && !IgnoreTarget && API.CanCast("Tiger Palm")) ;
                    //{
                    //    if (API.CanCast("Spinning Crane Kick", "target") && Energy > 50) ;

                    //}
                }
            }


            

            //=====================================================================================
            //Brewmaster
            //=====================================================================================
            if (API.Spec() == "Monk: Brewmaster")
            {

                //BrM Talents ---------------------------------------------------------------------
                bool ChiBurst = (API.Talents(1) == 1);
                bool EyeoftheTiger = (API.Talents(1) == 2);
                bool ChiWave = (API.Talents(1) == 3);
                bool ChiTorpedo = (API.Talents(2) == 1);
                bool TigersLust = (API.Talents(2) == 2);
                bool Celerity = (API.Talents(2) == 3);
                bool LightBrewing = (API.Talents(3) == 1);
                bool BlackOxBrew = (API.Talents(3) == 2);
                bool GiftoftheMists = (API.Talents(3) == 3);
                bool RingofPeace = (API.Talents(4) == 1);
                bool SummonBlackOxStatue = (API.Talents(4) == 2);
                bool LegSweep = (API.Talents(4) == 3);
                bool HealingElixir = (API.Talents(5) == 1);
                bool MysticVitality = (API.Talents(5) == 2);
                bool DampenHarm = (API.Talents(5) == 3);
                bool RushingJadeWind = (API.Talents(6) == 1);
                bool InvokeNiuzao = (API.Talents(6) == 2);
                bool SpecialDelivery = (API.Talents(6) == 3);
                bool ElusiveDance = (API.Talents(7) == 1);
                bool BlackoutCombo = (API.Talents(7) == 2);
                bool HighTolerance = (API.Talents(7) == 3);

                //BrM Macros ----------------------------------------------------------------------
                //API.CreateMacro("Self Effuse", "/cast [@player] Effuse");

                //if (!API.IsInCombat && TargetHealth <= 85 && Energy >= 50 && (API.CastingSpell("player") == 0))
                //{
                //    API.Cast("Self Effuse");
                //    return;
                //}

                //BrM Basic Rotation --------------------------------------------------------------
                //Managing Stagger ----------------------------------------------------------------
                if (API.HasDebuff("Light Stagger"))
                {
                    API.Cast("Purifying Brew");
                    return;
                }
                if (API.HasDebuff("Moderate Stagger")) && 
                {

                    return;
                }
                if (API.HasDebuff("Heavy Stagger"))
                {
                    API.Cast("Purifying Brew");
                    return;
                }

                if ((API.CanCast("Tiger Palm", "target")) && (!IgnoreTarget) && API.Spec() == "Monk: Brewmaster")
                {
                    if (API.CanCast("Rushing Jade Wind") && (API.BuffRemaining("Rushing Jade Wind") < 300))
                    {
                        API.Cast("Rushing Jade Wind");
                        return;
                    }

                    if ((API.CanCast("Keg Smash", "target")) && (Energy >= 40))
                    {
                            API.Cast("Keg Smash");
                            return;
                    }

                    if (API.CanCast("Blackout Strike", "target"))
                    {
                            API.Cast("Blackout Strike");
                            return;
                    }

                    if (API.CanCast("Breath of Fire", "target"))
                    {
                            API.Cast("Breath of Fire");
                            return;
                    }

                    if ((API.CanCast("Tiger Palm", "target")) && (Energy >= 65) && (API.SpellCooldown("Breath of Fire") < MaxGCD * .28) && (API.SpellCooldown("Keg Smash") < MaxGCD * .28) && (API.SpellCooldown("Blackout Strike") < MaxGCD * .28))
                    {
                        API.Cast("Tiger Palm");
                        return;
                    }
                }
            }


            /*
            //=====================================================================================
            //Mistweaver
            //=====================================================================================
            if (API.Spec() == "Monk: Mistweaver")
            {
                //MW Talents ----------------------------------------------------------------------
                bool ChiBurst = (API.Talents(1) == 1);
                bool ZenPulse = (API.Talents(1) == 2);
                bool ChiWave = (API.Talents(1) == 3);
                bool ChiTorpedo = (API.Talents(2) == 1);
                bool TigersLust = (API.Talents(2) == 2);
                bool Celerity = (API.Talents(2) == 3);
                bool Lifecycles = (API.Talents(3) == 1);
                bool SpiritoftheCrane = (API.Talents(3) == 2);
                bool MistWrap = (API.Talents(3) == 3);
                bool RingofPeace = (API.Talents(4) == 1);
                bool SongofChiJi = (API.Talents(4) == 2);
                bool LegSweep = (API.Talents(4) == 3);
                bool HealingElixir = (API.Talents(5) == 1);
                bool DiffuseMagic = (API.Talents(5) == 2);
                bool DampenHarm = (API.Talents(5) == 3);
                bool RefreshingJadeWind = (API.Talents(6) == 1);
                bool InvokeChiJi = (API.Talents(6) == 2);
                bool SummonJadeSerpent = (API.Talents(6) == 3);
                bool ManaTea = (API.Talents(7) == 1);
                bool FocusedThunder = (API.Talents(7) == 2);
                bool RisingThunder = (API.Talents(7) == 3);

                //MW Macros -----------------------------------------------------------------------
                //API.CreateMacro("Focus Effuse", "/cast [@focus] Effuse");

                //MW Rotation Start
                if ((API.CanCast("Effuse")) && API.Spec() == "Monk: Mistweaver"))
                {
                    //Heal if not in combat
                    if (API.IsInCombat && HealthPercent <= 95 & Energy >= 30)
                    {
                        //Macro("/cast [@player] Effuse");
                        return;
                    }
                } */              
        }        
    }
}