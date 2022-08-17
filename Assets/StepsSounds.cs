using SevenDays.unLOC.Core.Player.Animations;

using UnityEngine;

using Random = UnityEngine.Random;

public class StepsSounds : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] _stepsSounds;

    [SerializeField]
    private AnimationCallBackView _animationCallBackView;

    [SerializeField]
    private AudioSource _audioSource;

    private void Start()
    {
        _animationCallBackView.Step += PlaySound;
    }

    private void PlaySound()
    {
        _audioSource.clip = _stepsSounds[Random.Range(0, _stepsSounds.Length)];
        _audioSource.Play();
    }
}
