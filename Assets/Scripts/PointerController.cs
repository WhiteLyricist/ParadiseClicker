using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[Serializable]
public class Object
{
    public Image image;
    public int countObject;
}

public class PointerController : MonoBehaviour, IPointerDownHandler
{
    private const int _size = 5; 

    [SerializeField]
    private List<Object> objects = new List<Object>();

    private Queue<Object> objectsInScene = new Queue<Object>(_size);
    private Queue<Image> imageInScene = new Queue<Image>(_size);

    private Image _object;

    public void OnPointerDown(PointerEventData eventData)
    {
        InstantiateObject();
    }

    private void InstantiateObject() //Инициализация объектов
    {
        var spaunList = objects.Where(obj => obj.countObject > 0).ToList();

        if (spaunList.Count > 0)
        {
            var i = Random.Range(0, spaunList.Count);

            if (spaunList[i].countObject != 0) //Если объектов хватает в резерве
            {
                _object = Instantiate(spaunList[i].image) as Image;
                _object.transform.parent = gameObject.transform;
                _object.transform.position = Input.mousePosition;

                if (_size != objectsInScene.Count)
                {
                    spaunList[i].countObject -= 1;
                    objectsInScene.Enqueue(spaunList[i]);
                    imageInScene.Enqueue(_object);
                }
                else
                {
                    objectsInScene.Peek().countObject++;
                    objectsInScene.Dequeue();
                    Destroy(imageInScene.Dequeue());

                    spaunList[i].countObject -= 1;
                    objectsInScene.Enqueue(spaunList[i]);
                    imageInScene.Enqueue(_object);
                }
            }
        }
        else
        {

            objectsInScene.Enqueue(objectsInScene.Dequeue());

            imageInScene.Peek().transform.position = Input.mousePosition;
            imageInScene.Enqueue(imageInScene.Dequeue());
        }
    }
}
