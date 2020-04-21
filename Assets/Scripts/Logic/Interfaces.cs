using UnityEngine;
using System.Collections;

public interface ICharacter: IIdentifiable
{	

}

public interface IIdentifiable
{
    int ID { get; }
}
