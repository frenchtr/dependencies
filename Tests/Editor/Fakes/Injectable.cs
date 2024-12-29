using TravisRFrench.Dependencies.Runtime.Injection;
using UnityEngine;

namespace TravisRFrench.Dependencies.Tests.Editor.Fakes
{
    public class Injectable
    {
        public GameService GameServiceFromConstructor { get; }
        public GameObject PlayerFromConstructor { get; }
        [Inject]
        public GameService GameServiceFromInjectedField;
        [Inject]
        public GameObject PlayerFromInjectedField;
        [Inject]
        public GameService GameServiceFromInjectedProperty { get; private set; }
        [Inject]
        public GameObject PlayerFromInjectedProperty { get; private set; }
        public GameService GameServiceFromInjectedMethod { get; set; }
        public GameObject PlayerFromInjectedMethod { get; set; }
        public bool WasInjectedConstructorCalled { get; private set; }
        
        public Injectable()
        {
        }
        
        public Injectable(GameService gameServiceFromConstructor, GameObject playerFromConstructor)
        {
            this.GameServiceFromConstructor = gameServiceFromConstructor;
            this.PlayerFromConstructor = playerFromConstructor;
        }
        
        [Inject]
        public Injectable(GameService gameServiceFromConstructor)
        {
            this.GameServiceFromConstructor = gameServiceFromConstructor;
            this.WasInjectedConstructorCalled = true;
        }
        
        [Inject]
        public void Inject(GameService gameServiceFromInjectedMethod, GameObject playerFromInjectedMethod)
        {
            this.GameServiceFromInjectedMethod = gameServiceFromInjectedMethod;
            this.PlayerFromInjectedMethod = playerFromInjectedMethod;
        }
    }
}
