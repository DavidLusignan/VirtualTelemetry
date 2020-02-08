using System;
using System.Collections.Generic;
using System.Text;

namespace Global.Observable {
    public class Observer<T> : IObserver<T> {
        private Action<T> onNext;
        private Action<Exception> onError;
        private Action onCompleted;

        public Observer(Action<T> onNext, Action<Exception> onError = null, Action onCompleted = null) {
            this.onNext = onNext;
            this.onError = onError;
            this.onCompleted = onCompleted;
        }

        public void OnCompleted() {
            onCompleted?.Invoke();
        }

        public void OnError(Exception error) {
            onError?.Invoke(error);
        }

        public void OnNext(T value) {
            onNext?.Invoke(value);
        }
    }
}
