import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { UserService } from '../../core/services/user.service';


@Component({
  selector: 'app-login',
  imports: [],
  templateUrl: './login.html',
  styleUrl: './login.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class Login {
  private userService = inject(UserService);

}
