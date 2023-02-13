using Mixin.TheLastMove.Environment.Collectable;
using UnityEngine;

public class CollectableDataSet : MonoBehaviour
{
    [SerializeField]
    private CollectableOperator[] _collectables;

    public CollectableOperator[] Collectables { get => _collectables; set => _collectables = value; }
}
