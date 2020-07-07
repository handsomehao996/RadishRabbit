using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//玩家冲刺背影
public class PlayerBackView : MonoBehaviour
{
    public Transform player;
    public Color fadeInColor;
    public Color fadeOutColor;
    public float fadeTime = 0.2f;
    public float nextCreateTime = 0.2f;

    //背影图片、位置、翻转、颜色变化
    public void BackViewAppear()
    {
        Sequence s = DOTween.Sequence();
        for(int i = 0; i < transform.childCount; i++)
        {
            Transform BackViewSprite = transform.GetChild(i);
            //位置
            s.AppendCallback(() => BackViewSprite.position = player.position);
            //图片
            s.AppendCallback(() => BackViewSprite.GetComponent<SpriteRenderer>().sprite = player.GetComponent<SpriteRenderer>().sprite);
            //翻转
            s.AppendCallback(() => BackViewSprite.GetComponent<SpriteRenderer>().flipX = player.GetComponent<SpriteRenderer>().flipX);

            //颜色变化
            s.Append(BackViewSprite.GetComponent<SpriteRenderer>().material.DOColor(fadeInColor, 0));
            s.AppendCallback(() => FadeOut(BackViewSprite));
            s.AppendInterval(nextCreateTime);
        }
    }

    void FadeOut(Transform _BackViewSprite)
    {
        _BackViewSprite.GetComponent<SpriteRenderer>().material.DOKill();
        _BackViewSprite.GetComponent<SpriteRenderer>().material.DOColor(fadeOutColor, fadeTime);
    }
}
