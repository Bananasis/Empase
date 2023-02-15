using System;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.Events;

namespace Utils
{
    public abstract class PlayerPrefContainers<K, V>
    {
        protected readonly string _name;

        protected readonly V _def;
        protected readonly PlayerPrefInt _size;
        public int size => _size.val;
        public readonly UnityEvent<(K, V)> OnChange = new UnityEvent<(K, V)>();
        public readonly UnityEvent<(K, V)> OnRemove = new UnityEvent<(K, V)>();
        public readonly UnityEvent<(K, V)> OnAdd = new UnityEvent<(K, V)>();


        protected PlayerPrefContainers(string name, V def, int defSize = 0)
        {
            _def = def;
            _name = name;
            _size = new PlayerPrefInt(_name + "_size", defSize);
        }

        public abstract V this[K i] { get; set; }
        public abstract UnityEvent<V> GetEvent(K i);
    }

    public abstract class PlayerPrefList<S> : PlayerPrefContainers<int, S>
    {
        private readonly List<PlayerPrefContainer<S>> _prefContainers = new List<PlayerPrefContainer<S>>();

        protected PlayerPrefList(string name, S def, int defSize = 0) : base(name, def, defSize)
        {
            SetupContainers();
        }

        protected PlayerPrefList(string name, S def, IReadOnlyList<S> defValues = default) : base(name, def,
            defValues?.Count ?? 0)
        {
            SetupContainers(defValues);
        }

        public override S this[int i]
        {
            get => _prefContainers[i].val;
            set
            {
                if (_prefContainers[i].val.Equals(value)) return;
                _prefContainers[i].val = value;
                OnChange.Invoke((i, value));
            }
        }

        public override UnityEvent<S> GetEvent(int i)
        {
            return _prefContainers[i].OnChange;
        }

        protected void SetupContainers(IReadOnlyList<S> defValues)
        {
            for (int i = 0; i < size; i++)
            {
                _prefContainers.Add(GetContainer(_name + $"_{i}", defValues[i]));
            }
        }

        protected void SetupContainers()
        {
            for (int i = 0; i < size; i++)
            {
                _prefContainers.Add(GetContainer(_name + $"_{i}", _def));
            }
        }

        protected abstract PlayerPrefContainer<S> GetContainer(string name, S def);

        public void Add(S value)
        {
            OnAdd.Invoke((size, value));
            _size.val++;
            var cont = GetContainer(_name + $"_{size}", _def);
            cont.val = value;
            _prefContainers.Add(cont);
        }

        public void RemoveLast()
        {
            OnRemove.Invoke((size - 1, _prefContainers[size - 1].val));
            _size.val--;
            _prefContainers[size].Delete();
            _prefContainers.RemoveAt(size);
        }
    }

    public class PlayerPrefDictionary<K, V> : PlayerPrefContainers<K, V>
    {
        private Dictionary<K, int> keyPrefs = new Dictionary<K, int>();

        protected readonly List<PlayerPrefKeyValueContainer<K, V>> _prefContainers =
            new List<PlayerPrefKeyValueContainer<K, V>>();


        public PlayerPrefDictionary(string name, V def, IReadOnlyList<(K, V)> defValues = default) : base(name, def,
            defValues?.Count ?? 0)
        {
            SetupContainers(defValues);
            PopulatePrefDict();
        }

        public PlayerPrefDictionary(string name, V def, IReadOnlyList<K> defValues = default) : base(name, def,
            defValues?.Count ?? 0)
        {
            SetupContainers(defValues);
            PopulatePrefDict();
        }

        protected void SetupContainers(IReadOnlyList<K> defValues)
        {
            for (var i = 0; i < size; i++)
            {
                _prefContainers.Add(GetContainer(_name + $"_{i}", (defValues[i], _def)));
            }
        }

        protected void SetupContainers(IReadOnlyList<(K, V)> defValues)
        {
            for (var i = 0; i < size; i++)
            {
                _prefContainers.Add(GetContainer(_name + $"_{i}", defValues[i]));
            }
        }


        public override V this[K key]
        {
            get => _prefContainers[keyPrefs[key]].val.Item2;
            set
            {
                if (!keyPrefs.ContainsKey(key))
                {
                    Add(key, value);
                }

                int i = keyPrefs[key];
                var prefContainer = _prefContainers[keyPrefs[key]];
                if (prefContainer.val.Item2.Equals(value)) return;
                prefContainer.val = (key, value);
                OnChange.Invoke((key, value));
            }
        }

        private void Add(K key, V value)
        {
            _prefContainers.Add(GetContainer(_name + $"_{size}", (key, value)));
            keyPrefs[key] = size;
            OnAdd.Invoke((key, value));
            _size.val++;
        }

        public void Delete(K key)
        {
            int i = keyPrefs[key];
            keyPrefs.Remove(key);

            var temp = _prefContainers[size - 1];
            _prefContainers.RemoveAt(size - 1);
            OnRemove.Invoke(_prefContainers[i].val);
            _prefContainers[i] = temp;
            keyPrefs[temp.val.Item1] = i;
            _size.val--;
        }

        public override UnityEvent<V> GetEvent(K key)
        {
            return _prefContainers[keyPrefs[key]].OnValueChange;
        }


        protected PlayerPrefKeyValueContainer<K, V> GetContainer(string name, (K, V) def)
        {
            return new PlayerPrefKeyValueContainer<K, V>(name, def);
        }

        private void PopulatePrefDict()
        {
            for (var i = 0; i < _prefContainers.Count; i++)
            {
                var (key, _) = _prefContainers[i].val;
                if (keyPrefs.ContainsKey(key)) throw new GameException($"Pref container {_name} already contains key {key}");
                keyPrefs[key] = i;
            }
        }
    }

    public class PlayerPrefIntList : PlayerPrefList<int>
    {
        public PlayerPrefIntList(string name, int def, int defSize) : base(name, def, defSize)
        {
        }

        public PlayerPrefIntList(string name, int def, IReadOnlyList<int> defValues) : base(name, def, defValues)
        {
        }

        protected override PlayerPrefContainer<int> GetContainer(string name, int def)
        {
            return new PlayerPrefInt(name, def);
        }
    }

    public class PlayerPrefTypeList<S> : PlayerPrefList<S>
    {
        public PlayerPrefTypeList(string name, S def, int defSize) : base(name, def, defSize)
        {
        }

        public PlayerPrefTypeList(string name, S def, IReadOnlyList<S> defValues) : base(name, def, defValues)
        {
        }

        protected override PlayerPrefContainer<S> GetContainer(string name, S def)
        {
            return new PlayerPrefComplexContainer<S>(name, def);
        }
    }

    public class PlayerPrefEnumList<E> : PlayerPrefList<E> where E : unmanaged, Enum
    {
        public PlayerPrefEnumList(string name, E def, int defSize) : base(name, def, defSize)
        {
        }

        protected override PlayerPrefContainer<E> GetContainer(string name, E def)
        {
            return new PlayerPrefEnum<E>(name, def);
        }
    }

    public class PlayerPrefFloatList : PlayerPrefList<float>
    {
        public PlayerPrefFloatList(string name, float def, int defSize) : base(name, def, defSize)
        {
        }

        protected override PlayerPrefContainer<float> GetContainer(string name, float def)
        {
            return new PlayerPrefFloat(name, def);
        }
    }

    public class PlayerPrefStringList : PlayerPrefList<string>
    {
        public PlayerPrefStringList(string name, string def, int defSize) : base(name, def, defSize)
        {
        }

        protected override PlayerPrefContainer<string> GetContainer(string name, string def)
        {
            return new PlayerPrefString(name, def);
        }
    }

    public class PlayerPrefBoolList : PlayerPrefList<bool>
    {
        public PlayerPrefBoolList(string name, bool def, int defSize) : base(name, def, defSize)
        {
        }

        protected override PlayerPrefContainer<bool> GetContainer(string name, bool def)
        {
            return new PlayerPrefBool(name, def);
        }
    }

    public class PlayerPrefString : PlayerPrefContainer<string>
    {
        public override string val
        {
            get => _value;
            set
            {
                if (value == _value) return;
                _value = value;
                OnChange.Invoke(value);
                PlayerPrefs.SetString(_name, _value);
            }
        }

        public void Set(string value)
        {
            val = value;
        }

        public PlayerPrefString(string name, string def = default) : base(name)
        {
            _value = PlayerPrefs.GetString(name, def);
        }
    }

    public class PlayerPrefInt : PlayerPrefContainer<int>
    {
        public override int val
        {
            get => _value;
            set
            {
                if (value == _value) return;
                _value = value;
                OnChange.Invoke(value);
                PlayerPrefs.SetInt(_name, _value);
            }
        }

        public PlayerPrefInt(string name, int def = default) : base(name)
        {
            _value = PlayerPrefs.GetInt(name, def);
        }
    }

    public class PlayerPrefEnum<E> : PlayerPrefContainer<E> where E : unmanaged, Enum
    {
        public override E val
        {
            get => _value;
            set
            {
                unsafe
                {
                    if (value.Equals(_value)) return;
                    _value = value;
                    OnChange.Invoke(value);
                    var temp = _value;
                    PlayerPrefs.SetInt(_name, *(int*) (&temp));
                }
            }
        }

        public unsafe PlayerPrefEnum(string name, E def = default) : base(name)
        {
            int temp = PlayerPrefs.GetInt(name, *(int*) (&def));
            _value = *(E*) (&temp);
        }
    }

    public class PlayerPrefBool : PlayerPrefContainer<bool>
    {
        public override bool val
        {
            get => _value;
            set
            {
                if (value == _value) return;
                _value = value;
                OnChange.Invoke(value);
                PlayerPrefs.SetInt(_name, _value ? 1 : 0);
            }
        }

        public PlayerPrefBool(string name, bool def = default) : base(name)
        {
            _value = PlayerPrefs.GetInt(name, def ? 1 : 0) != 0;
        }
    }

    public class PlayerPrefFloat : PlayerPrefContainer<float>
    {
        public override float val
        {
            get => _value;
            set
            {
                if (value == _value) return;
                _value = value;
                OnChange.Invoke(value);
                PlayerPrefs.SetFloat(_name, _value);
            }
        }

        public PlayerPrefFloat(string name, float def = default) : base(name)
        {
            _value = PlayerPrefs.GetFloat(name, def);
        }
    }

    public abstract class PlayerPrefContainer<S>
    {
        protected readonly string _name;
        protected S _value;
        public readonly UnityEvent<S> OnChange;

        public abstract S val { get; set; }

        public void Delete()
        {
            PlayerPrefs.DeleteKey(_name);
        }

        public PlayerPrefContainer(string name, S def = default)
        {
            OnChange = new UnityEvent<S>();
            this._name = name;
        }
    }

    public class PlayerPrefComplexContainer<S> : PlayerPrefContainer<S>
    {
        private string _json;

        public override S val
        {
            get => _value;
            set
            {
                string newJson = JsonUtility.ToJson(value);
                if (newJson == _json) return;
                _value = value;
                OnChange.Invoke(value);
                _json = newJson;
                PlayerPrefs.SetString(_name, _json);
            }
        }
        
        

        public PlayerPrefComplexContainer(string name, S def = default) : base(name)
        {
            _json = JsonUtility.ToJson(def);
            _json = PlayerPrefs.GetString(name, _json);
            _value = JsonUtility.FromJson<S>(_json);
        }
    }

    public class PlayerPrefKeyValueContainer<K, V> : PlayerPrefComplexContainer<(K, V)>
    {
        public UnityEvent<V> OnValueChange = new UnityEvent<V>();
        public PlayerPrefKeyValueContainer(string name, (K, V) def = default) : base(name, def)
        {
            OnChange.AddListener((tuple) => OnValueChange.Invoke(tuple.Item2));
        }
    }

}