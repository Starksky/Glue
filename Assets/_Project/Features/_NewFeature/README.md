# NewFeature

            ## Правила взаимодействия слоёв

            ✅ РАЗРЕШЕНО:
              Controllers → View (через INewFeatureView)
              View → Presenter (через INewFeaturePresenter)
              Presenter → UseCase (через INewFeatureUseCase)
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
            