using System;
using UnityEngine;

namespace GRV.ToolsModule.BroTools
{

    public class UIElementAnimated : UIElement
    {

        public AnimationSettings animationSettings = new AnimationSettings();

        public Action ActionAfterHide { get; set; }
        public Action ActionAfterShow { get; set; }

        public bool IsPlaying { get; private set; }

        private string activePresetName;
        private BroTweener activePreset;

        protected virtual void OnDisable()
        {
            // Если элемент отключился, пока проигрывалась анимация закрытия, выключаем его
            if (!IsActive && IsPlaying) base.Hide();
        }

        public override void Show()
        {
            switch (animationSettings.source)
            {
                case AnimationSettings.SourceType.NotAnimated:
                    base.Show();
                    return;

                case AnimationSettings.SourceType.Preset:

                    // Если не сохранён, загружаем и добавляем в кэш
                    if (animationSettings.PresetName != activePresetName)
                    {
                        var preset = Resources.Load<BroTweener>($"TweenerPresets/{animationSettings.PresetName}");
                        if (preset)
                        {
                            if (activePreset) Destroy(activePreset.gameObject);
                            activePresetName = animationSettings.PresetName;
                            activePreset = Instantiate(preset, transform);
                            activePreset.Target = transform;
                            animationSettings.tweenerShow = activePreset;
                            animationSettings.splitTweeners = false;
                        } else
                        {
                            Debug.LogError($"Tweener preset \"Resources/TweenerPresets/{animationSettings.PresetName}\" is not exists");
                        }
                    }
                    break;
            }


            base.Show();

            if (animationSettings.tweenerShow == null) return;

            // Play Show tweener
            IsPlaying = true;
            animationSettings.tweenerShow.Play();
            animationSettings.tweenerShow.SetOnFinish(() =>
            {
                IsPlaying = false;
                ActionAfterShow?.Invoke();
            });
        }

        public override void Hide()
        {
            bool reversedShow = !animationSettings.splitTweeners;

            BroTweener tweenerHide = reversedShow ? animationSettings.tweenerShow : animationSettings.tweenerHide;

            // Если твинеров нет, закрываем как базовый класс
            if (!tweenerHide)
            {
                base.Hide();
                return;
            }

            // Элемент считается закрытым ещё до начала проигрывания анимации
            IsActive = false;

            IsPlaying = true;

            tweenerHide.PlayDirectly(reversedShow ? -1 : 1);
            tweenerHide.SetOnFinish(() =>
            {
                base.Hide();
                ActionAfterHide?.Invoke();
                IsPlaying = false;
            });
        }


        [System.Serializable]
        public class AnimationSettings
        {
            public enum SourceType { NotAnimated, Preset, Tweener }
            public SourceType source = SourceType.Preset;
            public const string presetNameDefault = "TweenerPreset_Default";
            public string presetName;
            public BroTweener tweenerShow;
            public BroTweener tweenerHide;
            public bool splitTweeners;

            public string PresetName => !string.IsNullOrEmpty(presetName) ? presetName : presetNameDefault;
        }
    }
}