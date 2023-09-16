using System.Collections;
using UnityEngine;

public enum ECookwareState
{
    Idle,
    Cook,
    Complete,
    Overheat
}

public abstract class Cookware : Container
{
    [SerializeField]
    protected CookingMethod cookingMethod;
    [SerializeField]
    private bool IsImmediateCook = true;

    protected ECookwareState cookwareState;

    protected float currProgressTime;

    private bool STOP = false;

    private void Awake()
    {
        Debug.Log("Cookware");
        cookwareState = ECookwareState.Idle;
    }

    private IEnumerator Cook()
    {
        // 조리 도구마다 방식이 다름 
        // 조리가 진행중인 음식이 들어올 수도 있음 = Get() 함수 override 해야할 것 
        Recipe recipe = RecipeManager.Instance.Search(cookingMethod, containObjects);
        Debug.Log(recipe);
        float cookDuration = recipe.getCookTime();
        currProgressTime = 0;

        while (currProgressTime < cookDuration)
        {
            if (STOP)
                yield break;

            currProgressTime += 0.1f;
            Debug.Log($"Progress: {currProgressTime} / {cookDuration}%");
            yield return new WaitForSeconds(0.1f);
        }

        LinkManager.Instance.GetLinkedPlayer(this).GetInteractor().RemoveObject(getObject);
        Destroy(getObject.gameObject);

        containObjects.Clear();
        getObject = Instantiate(recipe.getCookedFood().gameObject, transform.position + offset, Quaternion.identity).GetComponent<InteractableObject>();

        if (getObject == null)
        {
            Debug.Log("Invalid Component : 'Food'");
        }
        else
        {
            cookwareState = ECookwareState.Complete;
        }
        CompletedCook();
        LinkManager.Instance.Disconnect(this);
    }

    protected void StartCook()
    {
        STOP = false;
        cookwareState = ECookwareState.Cook;
        StartCoroutine(Cook());
    }

    protected void StopCook()
    {
        STOP = true;
    }

    public virtual bool Interact()
    {
        if (cookwareState != ECookwareState.Complete)
        {
            StartCook();
            return true;
        }
        return false;
    }

    public override void Put(InteractableObject interactableObject)
    {
        base.Put(interactableObject);
        Debug.Log("<color=orange> Cookware Put </color>");
         
        if (IsImmediateCook && IsFull())
        {
            StartCook();
        }
    }

    protected abstract void CompletedCook();
}
