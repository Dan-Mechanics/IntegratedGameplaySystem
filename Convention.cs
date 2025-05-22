using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
// Usings go here ^

namespace ProjectNameHere
{
    public class SomeClass 
    {
        public event Action MyAction;
        public event EventHandler MyEventHandler;

        /// <summary>
        /// Or just "new();" is also allowed.
        /// Prefer keeping dictionaries and lists private
        /// because even if readonly one could call list.Clear();.
        /// </summary>
        private readonly Dictionary<string, object> someDictionary = new Dictionary<string, object>();
        private readonly List<float> someList = new List<float>();

        public const float SOME_CONST_FLOAT = 10f;
        public float somePublicFloat;
        private float somePrivateFloat;
        private static float someStaticFloat;

        /// <summary>
        /// This looks strange to me.
        /// </summary>
        private float uIWidth; // UILength ? NO !!

        /// <summary>
        /// Dont try to fix it like this please.
        /// </summary>
        private float ui_width;

        /// <summary>
        /// Call it something else instead.
        /// </summary>
        private float displayLength;

        /// <summary>
        /// Ideally use default for SerializeField because it prevents an annoying warning.
        /// However, if the var is really annoying to set in the inspector consider
        /// setting a default value that is neutral, but don't rely on it.
        /// </summary>
        [SerializeField] private float someInspectorVar = default;
        [SerializeField] private Color someColor = Color.white;
        [SerializeField] private KeyCode someKey = KeyCode.Mouse0;

        /// <summary>
        /// UnityEvents and all collections go at the end.
        /// </summary>
        [SerializeField] private UnityEvent someUnityEvent = default;
        [SerializeField] private List<Example1> someAwsomeCollectionOfThings = default;

        /// <summary>
        /// Private fields go at the very end.
        /// </summary>
        private SomeInterface coolInterfaceUsage;

        private void DoSomePrivateVoid() 
        {
            float localFloat = 10f;
            localFloat = 5.5f;
            int localInt = 10;

            // If you must use normal comments instead of summaries
            // try to use normal capitalizaiton.

            // This is good.
            localInt = localFloat == 10f ? 0 : 1;

            // This is not.
            localInt = localFloat == 10f ? (localInt == 0 ? 1 : 2) : 1;

            if (localInt == 0)
                Debug.Log("This is good.");

            if (localInt == 0)
                Debug.Log("But this is not ideal. But this is not ideal. But this is not ideal. But this is not ideal. But this is not ideal. ");

            // Dont do this either.
            if (localInt == 0)
                for (int i = 0; i < 10; i++)
                {

                }

            if (localInt == 1)
            {
                Debug.Log("This is logged guard clause.");
                return;
            }

            // Guard clause. 
            if (localInt == 1)
                return;

            if (localInt == 2)
            {
                Debug.Log("This is good.");
            }
            else 
            {
                Debug.Log("This is good.");
            }

            // Prefer switch over else-if else-if else-if else-if

            switch (localInt)
            {
                case 1:
                    Debug.Log("This is good.");
                    break;
                case 2:
                    Debug.Log("You get it");
                    break;
                default:
                    break;
            }

            // Prefer using state machine or interfaces ( command pattern ) over switch statement.
            // Or perhaps a dictionary.
            coolInterfaceUsage?.DoSomethingAwsome();

            // Example:
            coolInterfaceUsage = new Example2();

            coolInterfaceUsage?.DoSomethingAwsome();
        }

        /// <summary>
        /// Ideally use summaries to describe code
        /// that is not entirely clear.
        /// The order of the methods does not really matter.
        /// </summary>
        public void DoSomePublicVoid() 
        {
            // Code here.
        }

        public virtual void SomeEmptyVirtualMethod() { }

        private float DoSomeTinyMethod() => somePrivateFloat;

        private void ThisIsAllowedIfTheLineIsntTooLong() => coolInterfaceUsage?.DoSomethingAwsome();
        private void SoThisGood() => coolInterfaceUsage?.DoSomethingAwsome();

        private void Dont_Name_It_Like_This() 
        {
            // Code here.
        }

        private void or_like_this() 
        {
            // Code here.
        }

        private void PleaseDontDoThisInThisLanguage() { 
            // Unless you are using C++ ?
            // Btw ! and ? and ... are allowed like that !
        }

        private void ThatsBetter() 
        {
            // Code here.
        }

        public class Example1 : SomeInterface
        {
            public void DoSomethingAwsome()
            {
                Debug.Log("This is good.");
            }
        }

        public class Example2 : SomeInterface
        {
            public void DoSomethingAwsome()
            {
                Debug.Log("You get it");
            }
        }
    }

    public interface SomeInterface 
    {
        void DoSomethingAwsome();
    }

    /// <summary>
    /// Make sure MonoBehaviours are always in their own script,
    /// you are only allowed to go two class or struct layers deep max.
    /// </summary>
    public class SomeBehaviourClass : MonoBehaviour
    {
        [SerializeField] private float varsGoHere = default;

        public event EventHandler<SomeEventArgs> SomethingBigHappened;

        /// <summary>
        /// You could place event args here, but you dont have to.
        /// Consider using structs and Actions instead.
        /// If you do, place the structs at the end like usual.
        /// </summary>
        public class SomeEventArgs : EventArgs 
        {
            public float someFloat;

            /// <summary>
            /// Don't have to give everything constructor,
            /// but do if it needs one.
            /// Do it this way ideally.
            /// </summary>
            public SomeEventArgs(float someFloat)
            {
                this.someFloat = someFloat;
            }
        }

        private void Start()
        {
            
        }

        private void Update()
        {
            
        }

        private void FixedUpdate()
        {
            
        }

        private void MyMethodsHere1() { }
        private void MyMethodsHere2() { }
        private void MyMethodsHere3() { }
        private void MyMethodsHere4() { }

        private void OnTriggerEnter(Collider other)
        {
            
        }

        private void OnDrawGizmos()
        {
            
        }

        public struct AddStructsAndClassesAtTheEnd 
        {

        }

        public class ThisIsStillAllowed 
        {
            public class ButThisIsNot 
            {

            }
        }
    }
}