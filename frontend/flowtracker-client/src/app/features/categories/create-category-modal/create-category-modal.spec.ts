import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateCategoryModal } from './create-category-modal';

describe('CreateCategoryModal', () => {
  let component: CreateCategoryModal;
  let fixture: ComponentFixture<CreateCategoryModal>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CreateCategoryModal]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreateCategoryModal);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
