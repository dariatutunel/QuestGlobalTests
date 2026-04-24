# E-Commerce Test Automation Suite | Playwright & C#

## Overview
This repository contains a comprehensive automated testing suite for the [Automation Exercise](https://automationexercise.com/) e-commerce platform. It serves as a proof-of-concept for end-to-end (E2E) testing, covering UI interactions, API background validation, mobile responsiveness and multi-tab scenarios.

**Tech Stack:** `C#`, `NUnit`, `Playwright`

## Key Testing Concepts Demonstrated
* **API Validation within UI Tests:** Using Playwright's `APIRequestContext` to validate background network responses.
* **Mobile Viewport Emulation:** Dynamically altering the browser context to simulate a mobile device and validating responsive UI elements like hamburger menus.
* **Multi-Tab Synchronization:** Sharing a single browser context across multiple pages to test real-time data synchronization (e.g., cart updates across tabs).
* **Smart Waiting Strategies:** Replacing hardcoded sleeps with dynamic assertions (`WaitForLoadStateAsync`, `Expect().ToHaveTextAsync()`).
* **Soft Assertions:** Using `Assert.Multiple` to aggregate non-critical UI errors without terminating the test execution prematurely.

## Automated Test Scenarios

* **Carousel Functionality** : Validates arrow controls and dynamic content rendering in the 'Recommended Items' carousel. Pass
* **Broken Links Check** : Scans the main navigation menu and validates HTTP response codes (200 OK) for all endpoints. Fail
* **Mobile Responsiveness** : Emulates an iPhone viewport to verify the main menu collapses into a mobile-friendly layout. Fail
* **Session Persistence** : Adds items to the cart, triggers a forced page reload, and validates that the shopping cart data persists. Pass 
* **Cross-Tab Sync** : Opens two simultaneous tabs, modifies the cart in Tab 1, and verifies real-time updates in Tab 2. Pass 

## Notable Bugs Found During Automation
1. **Server Crash on Logout:** The `/logout` endpoint returns an HTTP 500 Internal Server Error when accessed without an active user session.
2. **Responsive Design Failure:** When viewed on mobile dimensions, the desktop navigation menu fails to collapse into a hamburger menu, breaking the UI layout.
