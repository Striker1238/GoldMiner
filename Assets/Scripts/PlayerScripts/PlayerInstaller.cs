using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class PlayerInstaller : MonoInstaller
{
    [SerializeField] private PlayerInputSettings inputSettings;

    public override void InstallBindings()
    {
        Container.BindInstance(inputSettings).AsSingle();

#if UNITY_ANDROID || UNITY_IOS
        Container.Bind<IInputSystem>().To<MobileInput>().FromNew().AsSingle();
#else
        Container.Bind<IInputSystem>().To<DesktopInput>().FromNew().AsSingle();
#endif  
    }

}
