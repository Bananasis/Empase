using UnityEngine;
using UnityEngine.Events;


    public interface IInputProvider
    {
         UnityEvent<Vector2> OnScrollNoShift { get; }
         UnityEvent<Vector2> OnScrollShift { get; }
         UnityEvent<(Vector2, float, bool)> OnMousePress{ get; }
         UnityEvent<Vector2> OnMouseClick { get; }
    }

    public partial class MouseInput :IInputProvider
    {
        public UnityEvent<Vector2> OnScrollNoShift { get; } = new UnityEvent<Vector2>();

        public UnityEvent<Vector2> OnScrollShift { get; } = new UnityEvent<Vector2>();

        public UnityEvent<(Vector2, float, bool)> OnMousePress { get; } = new UnityEvent<(Vector2, float, bool)>();

        public UnityEvent<Vector2> OnMouseClick { get; } = new UnityEvent<Vector2>();
    }
