using _Project.Features.Surfaces.Finish.Scripts.Contracts;
using SaintsField;
using UnityEngine;
using VContainer;

namespace _Project.Features.Surfaces.Finish.Scripts.View
{
    public class FinishSurfaceView : MonoBehaviour
    {
        [SerializeField, Tag] private string tagTrigger;
        private IFinishSurfacePresenter _presenter;
    
        [Inject]
        public void Construct(IFinishSurfacePresenter presenter)
        {
            _presenter = presenter;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(tagTrigger))
                _presenter.OnFinish();
        }
    }
}
