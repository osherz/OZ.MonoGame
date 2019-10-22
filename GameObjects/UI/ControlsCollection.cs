using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace OZ.MonoGame.GameObjects.UI
{
    public class ControlsCollection : IList<Control>, IEnumerable<Control>, IGameObject
    {
        private List<Control> _controls;
        public GamePrototype GameParent { get; set; }
        public ControlsCollection(GamePrototype gameParent)
        {
            GameParent = gameParent;
            _controls = new List<Control>();
            
        }

        #region IList Implementation
        public Control this[int index] { get => ((IList<Control>)_controls)[index]; set => ((IList<Control>)_controls)[index] = value; }

        public int Count => ((IList<Control>)_controls).Count;

        public bool IsReadOnly => ((IList<Control>)_controls).IsReadOnly;

        public void Add(Control item)
        {
            ((IList<Control>)_controls).Add(item);
            if(!(GameParent is null) && GameParent.IsLoadContentHappened)
            {
                item.Initialize();
                item.LoadContent(GameParent.Content);
            }
            OnControlAdded();
        }

        public void Insert(Control item, int index)
        {
            ((IList<Control>)_controls).Insert(index, item);
            if (!(GameParent is null) && GameParent.IsLoadContentHappened)
            {
                item.Initialize();
                item.LoadContent(GameParent.Content);
            }
            OnControlAdded();
        }


        public void AddRange(params Control[] controls)
        {
            foreach (var control in controls)
            {
                Add(control);
            }
        }

        public void Clear()
        {
            while(_controls.Count > 0)
            {
                RemoveAt(0);
            }
            ((IList<Control>)_controls).Clear();
        }

        public bool Contains(Control item)
        {
            return ((IList<Control>)_controls).Contains(item);
        }

        public void CopyTo(Control[] array, int arrayIndex)
        {
            ((IList<Control>)_controls).CopyTo(array, arrayIndex);
        }

        public IEnumerator<Control> GetEnumerator()
        {
            return ((IList<Control>)_controls).GetEnumerator();
        }

        public int IndexOf(Control item)
        {
            return ((IList<Control>)_controls).IndexOf(item);
        }

        public void Insert(int index, Control item)
        {
            ((IList<Control>)_controls).Insert(index, item);
            if (!(GameParent is null) && GameParent.IsLoadContentHappened)
            {
                item.Initialize();
                item.LoadContent(GameParent.Content);
            }

            OnControlAdded();
        }

        public bool Remove(Control item)
        {
            bool result = ((IList<Control>)_controls).Remove(item);
            if(result)
            {
                if (!(GameParent is null) && GameParent.IsLoadContentHappened)
                {
                    item.Initialize();
                    item.LoadContent(GameParent.Content);
                }

                OnControlRemoved();
            }
            return result;
        }

        public void RemoveAt(int index)
        {
            ((IList<Control>)_controls).RemoveAt(index);
            OnControlRemoved();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IList<Control>)_controls).GetEnumerator();
        }
        #endregion IList Implementation

        #region Events
        public event EventHandler ControlAdded;
        public event EventHandler ControlRemoved;
        #endregion Events

        #region Events Methods
        protected virtual void OnControlAdded()
        {
            ControlAdded?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnControlRemoved()
        {
            ControlRemoved?.Invoke(this, EventArgs.Empty);
        }
        #endregion Events Methods

        #region IGameObject Implementation
        public virtual void Initialize()
        {
            foreach (var control in _controls)
            {
                control.Initialize();
            }
        }

        public virtual void LoadContent(ContentManager content)
        {
            foreach (var control in _controls)
            {
                control.LoadContent(content);
            }
        }

        public virtual void UnloadContent(ContentManager content)
        {
            foreach (var control in _controls)
            {
                control?.UnloadContent(content);
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            foreach (var control in _controls)
            {
                control?.Update(gameTime);
            }
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var control in _controls)
            {
                control?.Draw(gameTime, spriteBatch);
            }
        }
        #endregion IGameObject Implementation


    }
}