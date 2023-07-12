import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TabEvaluacionesPage } from './tab-evaluaciones.page';

describe('TabEvaluacionesPage', () => {
  let component: TabEvaluacionesPage;
  let fixture: ComponentFixture<TabEvaluacionesPage>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TabEvaluacionesPage ],
      schemas: [CUSTOM_ELEMENTS_SCHEMA],
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TabEvaluacionesPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
