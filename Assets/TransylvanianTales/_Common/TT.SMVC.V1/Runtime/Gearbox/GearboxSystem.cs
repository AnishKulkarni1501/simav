using System.Collections;
using TT.Core.MonoBehaviourComponentSystem.V1;
using UnityEngine;
using UnityEngine.Events;

namespace TT.SMVC.V1
{
    [AddComponentMenu("Transylvanian Tales/Simple Modular Vehicle Controller/V1/Gearbox Module/Gearbox System")]
    public class GearboxSystem : GenericSystemBase<GearboxSystem>
    {
        public override bool PersistBetweenScenes => true;

        public UnityAction<Gearbox> OnGearStartedToChange;
        public UnityAction<Gearbox> OnGearFinishedChanging;


        void OnEnable()
        {
            Gearbox.Registry.OnComponentRegistered += GearboxRegistered;
        }

        void OnDisable()
        {
            Gearbox.Registry.OnComponentRegistered -= GearboxRegistered;
        }

        void Update()
        {
            ProcessGearboxes();
        }


        void GearboxRegistered(Gearbox gearbox)
        {
            InitializeGearbox(gearbox);
        }


        void InitializeGearbox(Gearbox gearbox)
        {
            gearbox.currentGearValue = Mathf.Clamp(gearbox.configuration.currentGearInAwake, -1, gearbox.configuration.DGears.Length);
            UpdateCurrentGear(gearbox);
            gearbox.waitForGearShiftTime = new WaitForSeconds(gearbox.configuration.gearShiftTime);
        }

        void UpdateCurrentGear(Gearbox gearbox)
        {
            gearbox.currentGear = gearbox.currentGearValue > 0 ?
                gearbox.configuration.DGears[gearbox.currentGearValue - 1] :
                gearbox.currentGearValue == 0 ? gearbox.configuration.NGear :
                gearbox.configuration.RGear;
        }


        void ProcessGearboxes()
        {
            foreach (var gearbox in Gearbox.Registry.Components)
            {
                ProcessGearbox(gearbox);
            }
        }

        void ProcessGearbox(Gearbox gearbox)
        {
            if (!gearbox.isChangingGear)
            {
                TryShiftUp(gearbox);
                TryShiftDown(gearbox);
            }
        }

        void TryShiftUp(Gearbox gearbox)
        {
            if (gearbox.currentGearValue > 0 && gearbox.speedometer.speedKPH >
                gearbox.configuration.DGears[gearbox.currentGearValue - 1].targetSpeedForNextGear &&
                gearbox.engine.engineRPM >= gearbox.configuration.shiftUpRPM)
            {
                gearbox.changeGearCoroutine = StartCoroutine(ChangeGearAfterDelay(gearbox, gearbox.currentGearValue + 1));
            }
        }

        void TryShiftDown(Gearbox gearbox)
        {
            if (gearbox.currentGearValue > 1 && gearbox.speedometer.speedKPH <
                gearbox.configuration.DGears[gearbox.currentGearValue - 1].targetSpeedForPreviousGear &&
                gearbox.engine.engineRPM <= gearbox.configuration.shiftDownRPM)
            {
                gearbox.changeGearCoroutine = StartCoroutine(ChangeGearAfterDelay(gearbox, gearbox.currentGearValue - 1));
            }
        }


        IEnumerator ChangeGearAfterDelay(Gearbox gearbox, int value)
        {
            if (value >= -1 && value <= gearbox.configuration.DGears.Length)
            {
                gearbox.isChangingGear = true;
                OnGearStartedToChange?.Invoke(gearbox);

                yield return gearbox.waitForGearShiftTime;

                gearbox.currentGearValue = value;
                UpdateCurrentGear(gearbox);

                gearbox.changeGearCoroutine = null;
                gearbox.isChangingGear = false;
                OnGearFinishedChanging?.Invoke(gearbox);
            }
        }


        public void SetGearTo(Gearbox gearbox, int value)
        {
            if (gearbox == null || value < -1 || value > gearbox.configuration.DGears.Length) return;
            if (!gearbox.configuration.allowShiftIntoSameGear && gearbox.currentGearValue == value) return;

            if (gearbox.configuration.stopCurrentShiftWhenNewShift)
            {
                StopChangingGear(gearbox);
            }

            if (!gearbox.isChangingGear)
            {
                gearbox.currentGearValue = value;
                UpdateCurrentGear(gearbox);

                OnGearFinishedChanging?.Invoke(gearbox);
            }
        }

        public void SetGearInstantlyTo(Gearbox gearbox, int value)
        {
            if (gearbox == null) return;

            gearbox.currentGearValue = value;
            UpdateCurrentGear(gearbox);
            OnGearFinishedChanging?.Invoke(gearbox);
        }

        public void StopChangingGear(Gearbox gearbox)
        {
            if (gearbox == null || !gearbox.isChangingGear) return;

            if (gearbox.changeGearCoroutine != null)
            {
                StopCoroutine(gearbox.changeGearCoroutine);
                gearbox.changeGearCoroutine = null;
            }

            gearbox.isChangingGear = false;
        }
    }
}