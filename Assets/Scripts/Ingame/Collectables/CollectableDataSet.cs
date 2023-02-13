using Mixin.TheLastMove.Environment.Collectable;
using UnityEngine;

public class CollectableDataSet : MonoBehaviour
{
    [SerializeField]
    private Collectable[] _collectables;

    public Collectable[] Collectables { get => _collectables; set => _collectables = value; }
}
