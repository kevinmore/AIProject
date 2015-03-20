using UnityEngine;
using System.Collections;

namespace CS7056_AIToolKit
{
    public abstract class StateController : MonoBehaviour
    {
        public FSM myStateMachine;
        public string previousState;
        public string currentState;
        public float delayOffTime = 2.0f;
        private bool repeating = true;
        private float timeLeft = 0;
        public bool tickerOn = true;
        public string tickerEventName = "tick";
        public float totalTimeInSeconds = 1;
        public int startStateID = 0;
        public abstract void TickFired();



        //----->TIMER FUNCTIONS<---------//
        /// <summary>
        /// Turns the timer on.
        /// </summary>
        public void TurnOn()
        {
            timeLeft = totalTimeInSeconds;
            tickerOn = true;
        }//----------------------------------------


        //----------------------------------------
        /// <summary>
        /// Turns the timer off.
        /// </summary>
        public void TurnOff()
        {
            tickerOn = false;
        }//----------------------------------------


        //----------------------------------------
        /// <summary>
        /// Starts the new repeating timer.
        /// </summary>
        /// <param name="theTarget">The target.</param>
        /// <param name="theTargetMethod">The target method.</param>
        /// <param name="length">Length.</param>
        public void StartNewRepeatingTimer(GameObject theTarget, string theTargetMethod, float length)
        {
            repeating = true;
            totalTimeInSeconds = length;
            TurnOn();
        }//----------------------------------------	


        //----------------------------------------
        /// <summary>
        /// Starts the new timer.
        /// </summary>
        /// <param name="theTarget">The target.</param>
        /// <param name="theTargetMethod">The target method.</param>
        /// <param name="length">Length.</param>
        public void StartNewTimer(GameObject theTarget, string theTargetMethod, float length)
        {
            totalTimeInSeconds = length;
            TurnOn();
        }//----------------------------------------

        //----------------------------------------
        /// <summary>
        /// update from the extended class.
        /// </summary>
        public void SuperUpdate()
        {
            FireTimer();
        }//----------------------------------------


        //----------------------------------------
        /// <summary>
        /// Fires the timer.
        /// </summary>
        private void FireTimer()
        {

            if (tickerOn)
            {
                //Debug.Log(".."+timeLeft);
                timeLeft -= Time.deltaTime;

                if (timeLeft < 0)
                {
                    timeLeft = totalTimeInSeconds;
                    eventToFSM = "tick";
                    TickFired();

                    if (repeating) TurnOn();
                    else
                    {
                        TurnOff();
                        GameObject.DestroyImmediate(gameObject);
                    }
                }

            }
        }//----------------------------------------



        //----------------------------------------
        /// <summary>
        /// state transition callback
        /// </summary>
        public void EnteredState(string stateName)
        {
            currentState = stateName;
            Invoke("Entered_State_" + stateName, 0.0f);
        }
        //----------------------------------------


        //----------------------------------------
        /// <summary>
        /// Leaves the state.
        /// </summary>
        /// <param name="stateName">State name.</param>
        public void LeftState(string stateName)
        {
            previousState = stateName;
            Invoke("Left_State_" + stateName, 0.0f);
        }//----------------------------------------

        //----------------------------------------
        /// <summary>
        /// Recieves the event.
        /// </summary>
        /// <param name="event_">Event_.</param>
        public void RecieveEvent(string event_)
        {
            //Debug.Log("clicked "+event_);
            myStateMachine.OnEvent(event_);
        }//----------------------------------------

        //-----------------------------------------------------------
        /// <summary>
        /// Sends the event to FSM
        /// </summary>
        /// <value>The event to FS.</value>
        public string eventToFSM
        {
            set
            {
                myStateMachine.OnEvent(value);
            }
        }
        //-----------------------------------------------------------

        public void TimerFired()
        {
            eventToFSM = tickerEventName;
        }

    }
}