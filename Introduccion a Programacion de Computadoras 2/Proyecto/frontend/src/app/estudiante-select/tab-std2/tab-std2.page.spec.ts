import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TabStd2Page } from './tab-std2.page';

describe('TabStd2Page', () => {
  let component: TabStd2Page;
  let fixture: ComponentFixture<TabStd2Page>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TabStd2Page ],
      schemas: [CUSTOM_ELEMENTS_SCHEMA],
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TabStd2Page);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
