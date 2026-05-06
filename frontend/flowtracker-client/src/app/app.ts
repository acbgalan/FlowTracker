import { Component } from '@angular/core';
import { Layout } from "./shared/components/layout/layout";
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = 'flowtracker-client';
}
