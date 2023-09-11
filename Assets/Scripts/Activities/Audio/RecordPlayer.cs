using System;
using System.Collections.Generic;
using System.Threading;

using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace SevenDays.unLOC.Activities.Audio
{
    public class RecordPlayer : MonoBehaviour
    {
        [SerializeField]
        private Button _nextButton;

        [SerializeField]
        private Button _previousButton;

        [SerializeField]
        private Button _pauseButton;

        [SerializeField]
        private Button _playButton;

        [SerializeField]
        private TextMeshProUGUI _trackText;

        [SerializeField]
        private AudioSource _source;

        [SerializeField]
        private AudioData[] _audioData;

        private LinkedList<AudioData> _trackList;
        private LinkedListNode<AudioData> _trackListNode;

        private CancellationTokenSource _cts;

        private void Awake()
        {
            _trackList = new LinkedList<AudioData>(_audioData);

            _trackListNode = _trackList.First;

            _nextButton.OnClickAsAsyncEnumerable().Subscribe(PlayNext);

            _previousButton.OnClickAsAsyncEnumerable().Subscribe(PlayPrevious);

            _pauseButton.OnClickAsAsyncEnumerable().Subscribe(Pause);

            _playButton.OnClickAsAsyncEnumerable().Subscribe(UnPause);
        }

        private void Start()
        {
            PlayAsync().Forget();
        }

        private void PlayNext(AsyncUnit _)
        {
            _trackListNode = _trackListNode.Next ?? _trackList.First;

            PlayAsync().Forget();
        }

        private void PlayPrevious(AsyncUnit _)
        {
            _trackListNode = _trackListNode.Previous ?? _trackList.Last;

            PlayAsync().Forget();
        }

        private void Pause(AsyncUnit _)
        {
            CancelToken();
            _source.Pause();

            _pauseButton.gameObject.SetActive(false);
            _playButton.gameObject.SetActive(true);
        }

        private void UnPause(AsyncUnit _)
        {
            CancelToken();
            _source.UnPause();

            _pauseButton.gameObject.SetActive(true);
            _playButton.gameObject.SetActive(false);
        }

        private async UniTask PlayAsync()
        {
            CancelToken();

            using (_cts = new CancellationTokenSource())
            {
                var clip = _trackListNode.Value.Clip;

                _trackText.text = _trackListNode.Value.Name;

                _source.clip = clip;

                _source.Play();

                await UniTask.Delay(TimeSpan.FromSeconds(clip.length), cancellationToken: _cts.Token);

                PlayNext(default);
            }
        }

        private void CancelToken()
        {
            if (_cts != null)
            {
                _cts.Cancel();
                _cts = null;
            }
        }

        [Serializable]
        private class AudioData
        {
            public AudioClip Clip => _clip;

            public string Name => _name;

            [SerializeField]
            private AudioClip _clip;

            [SerializeField]
            private string _name;
        }
    }
}