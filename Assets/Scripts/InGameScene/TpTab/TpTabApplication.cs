using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TpTab
{
    public class TpTabApplication : BaseTabApplication
    {
        [SerializeField] private BaseTabController<TpTabApplication>[] _controllers;
        [SerializeField] private TpTabView _tpTabView;

        public TpTabView TpTabView
        {
            get
            {
                return _tpTabView;
            }
        }

        [SerializeField] private TpTabModel _tpTabModel;
        public TpTabModel TpTabModel
        {
            get
            {
                return _tpTabModel;
            }
        }

        public override void Init()
        {
            base.Init();
        }

        public override void OnEnter()
        {
            base.OnEnter();

            _tpTabModel.TPU01NameText.text = (string)DefinitionManager.Instance.DefinitionDic["TPU01NameText"];
            _tpTabModel.TPU01EffectText.text = (string)DefinitionManager.Instance.DefinitionDic["TPU01EffectText"];
            _tpTabModel.TPU02NameText.text = (string)DefinitionManager.Instance.DefinitionDic["TPU02NameText"];
            _tpTabModel.TPU02EffectText.text = (string)DefinitionManager.Instance.DefinitionDic["TPU02EffectText"];
            _tpTabModel.TPU03NameText.text = (string)DefinitionManager.Instance.DefinitionDic["TPU03NameText"];
            _tpTabModel.TPU03EffectText.text = (string)DefinitionManager.Instance.DefinitionDic["TPU03EffectText"];
            _tpTabModel.TPU04NameText.text = (string)DefinitionManager.Instance.DefinitionDic["TPU04NameText"];
            _tpTabModel.TPU04EffectText.text = (string)DefinitionManager.Instance.DefinitionDic["TPU04EffectText"];
            _tpTabModel.TPU05NameText.text = (string)DefinitionManager.Instance.DefinitionDic["TPU05NameText"];
            _tpTabModel.TPU05EffectText.text = (string)DefinitionManager.Instance.DefinitionDic["TPU05EffectText"];
            _tpTabModel.TPU06NameText.text = (string)DefinitionManager.Instance.DefinitionDic["TPU06NameText"];
            _tpTabModel.TPU06EffectText.text = (string)DefinitionManager.Instance.DefinitionDic["TPU06EffectText"];
            _tpTabModel.TPU07NameText.text = (string)DefinitionManager.Instance.DefinitionDic["TPU07NameText"];
            _tpTabModel.TPU07EffectText.text = (string)DefinitionManager.Instance.DefinitionDic["TPU07EffectText"];
            _tpTabModel.TPU08NameText.text = (string)DefinitionManager.Instance.DefinitionDic["TPU08NameText"];
            _tpTabModel.TPU08EffectText.text = (string)DefinitionManager.Instance.DefinitionDic["TPU08EffectText"];
        }

        public override void AdvanceTime(float dt_sec)
        {
            base.AdvanceTime(dt_sec);
        }

        public override void LateAdvanceTime(float dt_sec)
        {
            base.LateAdvanceTime(dt_sec);
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
