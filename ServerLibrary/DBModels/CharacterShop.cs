using Library;
using MirDB;
using Server.Envir;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.DBModels
{
    [UserObject]
    public sealed class CharacterShop : DBObject
    {
        [Association("CharacterShop")]
        public AccountInfo Account   //账号
        {
            get { return _Account; }
            set
            {
                if (_Account == value) return;

                var oldValue = _Account;
                _Account = value;

                OnChanged(oldValue, value, "Account");
            }
        }
        private AccountInfo _Account;

        public DateTime Date   //挂单时间
        {
            get { return _Date; }
            set
            {
                if (_Date == value) return;

                var oldValue = _Date;
                _Date = value;

                OnChanged(oldValue, value, "Date");
            }
        }
        private DateTime _Date;

        public CharacterInfo Character   //角色
        {
            get { return _Character; }
            set
            {
                if (_Character == value) return;

                var oldValue = _Character;
                _Character = value;

                OnChanged(oldValue, value, "Character");
            }
        }
        private CharacterInfo _Character;

        public long Price   //价格
        {
            get { return _Price; }
            set
            {
                if (_Price == value) return;

                var oldValue = _Price;
                _Price = value;

                OnChanged(oldValue, value, "Price");
            }
        }
        private long _Price;

        public CurrencyType PriceType   //价格类型
        {
            get { return _PriceType; }
            set
            {
                if (_PriceType == value) return;

                var oldValue = _PriceType;
                _PriceType = value;

                OnChanged(oldValue, value, "PriceType");
            }
        }

        private CurrencyType _PriceType;

        public bool IsSell
        {
            get { return _IsSell; }
            set
            {
                if (_IsSell == value) return;

                var oldValue = _IsSell;
                _IsSell = value;

                OnChanged(oldValue, value, "IsSell");
            }

        }
        private bool _IsSell;

        [Association("CharacterShop")]
        public AccountInfo BuyAccount   //账号
        {
            get { return _BuyAccount; }
            set
            {
                if (_BuyAccount == value) return;

                var oldValue = _BuyAccount;
                _BuyAccount = value;

                OnChanged(oldValue, value, "BuyAccount");
            }
        }
        private AccountInfo _BuyAccount;

        public DateTime BuyDate   //挂单时间
        {
            get { return _BuyDate; }
            set
            {
                if (_BuyDate == value) return;

                var oldValue = _BuyDate;
                _BuyDate = value;

                OnChanged(oldValue, value, "BuyDate");
            }
        }
        private DateTime _BuyDate;

        protected override internal void OnCreated()   //创建时
        {
            base.OnCreated();
            
            if (Account == null)
                throw new InvalidOperationException("CharacterShop 必须设置 Account");
                
            if (Character == null)
                throw new InvalidOperationException("CharacterShop 必须设置 Character");
                
            Date = SEnvir.Now;
            IsSell = true;  // 默认设置为可出售状态
            PriceType = CurrencyType.GameGold;  // 默认使用游戏币
        }

        protected override internal void OnSaved()   //保存时
        {
            try
            {
                base.OnSaved();

                // 验证必要的属性
                var errors = new List<string>();
                
                if (Account == null)
                    errors.Add("Account 为空");
                    
                if (Character == null)
                    errors.Add("Character 为空");
                    
                if (Price <= 0)
                    errors.Add($"价格无效: {Price}");
                    
                if (Date == DateTime.MinValue)
                    errors.Add("日期未设置");

                // 如果是已售出状态，验证购买相关信息
                if (!IsSell)
                {
                    if (BuyAccount == null)
                        errors.Add("已售出状态下 BuyAccount 为空");
                        
                    if (BuyDate == DateTime.MinValue)
                        errors.Add("已售出状态下 BuyDate 未设置");
                }

                if (errors.Any())
                {
                    System.Diagnostics.Debug.WriteLine($"CharacterShop[{Index}] 保存验证失败: {string.Join(", ", errors)}");
                    throw new InvalidOperationException($"CharacterShop[{Index}] 保存验证失败: {string.Join(", ", errors)}");
                }

                System.Diagnostics.Debug.WriteLine($"CharacterShop[{Index}] 保存成功: Account={Account?.Index}, Character={Character?.Index}, Price={Price}, IsSell={IsSell}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"CharacterShop[{Index}] OnSaved 异常: {ex.Message}");
                throw;
            }
        }

        protected override internal void OnLoaded()   //加载时
        {
            base.OnLoaded();

            // 确保基本属性已设置
            if (Date == DateTime.MinValue)
                Date = SEnvir.Now;

            if (Price <= 0)
                throw new InvalidOperationException($"CharacterShop[{Index}] 加载时发现无效的价格");
        }
    }
}
