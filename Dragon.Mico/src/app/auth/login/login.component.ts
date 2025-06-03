import { Component, OnInit } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';

import { OwlOptions } from 'ngx-owl-carousel-o';
import { AuthService } from 'src/app/core/services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})

export class LoginComponent implements OnInit {
  year: number = new Date().getFullYear();
  loginForm: UntypedFormGroup = this.formBuilder.group({
    username: ['', [Validators.required, Validators.email]],
    password: ['', Validators.required],
  });
  submitted = false;
  error = '';
  returnUrl!: string;
  showNavigationArrows: any;
  fieldTextType!: boolean;
  carouselOption: OwlOptions = { items: 1, loop: false, margin: 0, nav: false, dots: true, responsive: { 680: { items: 1 } } };

  constructor(private formBuilder: UntypedFormBuilder, private authService: AuthService, private router: Router) { }

  ngOnInit(): void {
    document.body.setAttribute('data-layout', 'vertical');
  }

  get f() { return this.loginForm.controls; }

  onSubmit() {
    this.submitted = true;
    if (this.loginForm.valid) {
      this.authService.login(this.loginForm.value);
    }
  }

  toggleFieldTextType() {
    this.fieldTextType = !this.fieldTextType;
  }
}
