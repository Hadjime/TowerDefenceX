using System.Collections.Generic;

namespace GRV.ToolsModule.BroTools
{

    public static class TweensConfig
    {
        public readonly static Dictionary<Tween, Config> tweensTable = new Dictionary<Tween, Config>
        {
            [Tween.Position] = new Config<TweenPosition>()
            {
                valuesCouldBeSetFromContext = true,
                hasAdditionMode = true,
            },
            [Tween.Rotation] = new Config<TweenRotation>()
            {
                valuesCouldBeSetFromContext = true,
                hasAdditionMode = true,
            },
            [Tween.Scale] = new Config<TweenScale>()
            {
                valuesCouldBeSetFromContext = true,
                hasAdditionMode = true,
            },
            [Tween.Width] = new Config<TweenWidth>()
            {
                valuesCouldBeSetFromContext = true,
                hasAdditionMode = true,
            },
            [Tween.Height] = new Config<TweenHeight>()
            {
                valuesCouldBeSetFromContext = true,
                hasAdditionMode = true,
            },
            [Tween.Color] = new Config<TweenColor>()
            {

            },
            [Tween.Alpha] = new Config<TweenAlpha>()
            {

            },
            [Tween.Link] = new Config<TweenLink>()
            {

            },
            [Tween.Action] = new Config<TweenAction>()
            {

            },
        };


        public abstract class Config
        {
            public abstract ITween GetBehaviourInstance();

            public bool valuesCouldBeSetFromContext;
            public bool hasAdditionMode;
        }
        public class Config<T> : Config where T : ITween, new()
        {
            public override ITween GetBehaviourInstance() => new T();
        }

    }
}