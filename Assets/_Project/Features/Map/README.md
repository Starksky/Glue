# Map

            ## Правила взаимодействия слоёв

            ✅ РАЗРЕШЕНО:
              Controllers → View (через IMapView)
              View → Presenter (через IMapPresenter)
              Presenter → UseCase (через IMapUseCase)
              UseCase → Model

            ❌ ЗАПРЕЩЕНО:
              View → UseCase (прыжок через слой)
              View → Model
              Presenter → Model (пропущен UseCase)
              Controller → Presenter (пропущен View)

            ## Внешнее взаимодействие
              С другими фичами — только через сигналы (Common.Signals)
              View-контакты — через Common.Contracts

            ## Структура
              Scripts/
              ├── Domain/
              ├── Application/
              ├── Presentation/
              ├── View/
              ├── Contracts/
              ├── Controllers/
              └── Infrastructure/
            