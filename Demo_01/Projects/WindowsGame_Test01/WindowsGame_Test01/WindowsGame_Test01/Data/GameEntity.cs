using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsGame_Test01.Helper;

namespace WindowsGame_Test01.Data
{
    public abstract class State<EntityType>
    {
        public virtual void Enter(EntityType obj) { }
        public virtual void Update(EntityType obj) { }
        public virtual void Exit(EntityType obj) { }
    }

    public class State_Disable : State<GameEntity> 
    {
        private State_Disable() { }
        private static State_Disable state;
        public static State_Disable Instance
        {
            get
            {
                if (state == null)
                    state = new State_Disable();
                return state;
            }
        }
        public override void Enter(GameEntity obj)
        {
            LogHelper.Write(obj.name+" Enter Disable Game State.");
        }
        public override void Exit(GameEntity obj)
        {
            LogHelper.Write(obj.name + " Exit Disable Game State.");
        }
        public override void Update(GameEntity obj)
        {
            LogHelper.Write(obj.name + " Update Disable Game State.");
        }
        
    }

    public class State_Enable : State<GameEntity> 
    {
        private State_Enable() { }
        private static State_Enable state;
        public static State_Enable Instance
        {
            get
            {
                if (state == null)
                    state = new State_Enable();
                return state;
            }
        }
        public override void Enter(GameEntity obj)
        {
            LogHelper.Write(obj.name + " Enter Enable Game State.");
        }
        public override void Exit(GameEntity obj)
        {
            LogHelper.Write(obj.name + " Exit Enable Game State.");
        }
        public override void Update(GameEntity obj)
        {
            LogHelper.Write(obj.name + " Update Enable Game State.");
        }

    }

    public class State_Dead : State<GameEntity> 
    {
        private State_Dead() { }
        private static State_Dead state;
        public static State_Dead Instance
        {
            get
            {
                if (state == null)
                    state = new State_Dead();
                return state;
            }
        }
        public override void Enter(GameEntity obj)
        {
            LogHelper.Write(obj.name + " Enter Dead Game State.");
        }
    }

    public class GameEntity
    {
        public readonly int Id;
        public State<GameEntity> currentState;
        public string name;

        public GameEntity(string setName) 
        {
            byte[] buffer = Guid.NewGuid().ToByteArray();  
            Id = BitConverter.ToInt32(buffer, 0);
            name = setName;
            currentState = State_Disable.Instance;
        }
        public void ChangeState(State<GameEntity> newState)
        {
            currentState.Exit(this);
            currentState = newState;
            newState.Enter(this);
        }
        public void Update()
        {
            currentState.Update(this);
        }
    }



}
